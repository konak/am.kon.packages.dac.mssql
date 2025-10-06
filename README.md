# am.kon.packages.dac.mssql

`am.kon.packages.dac.mssql` is a thin helper around `Microsoft.Data.SqlClient` that implements the `IDataBase` contract from `am.kon.packages.dac.primitives`. It streamlines executing commands, materialising `DataSet`/`DataTable` objects, and orchestrating transactional work without giving up control over the underlying ADO.NET primitives.

## Installation

```bash
 dotnet add package am.kon.packages.dac.mssql
```

## Creating a database instance

```csharp
using am.kon.packages.dac.mssql;

var database = new DataBase(
    connectionString: configuration.GetConnectionString("Reporting"),
    cancellationToken: appLifetime.ApplicationStopping);
```

The cancellation token is honoured by every async operation so you can signal shutdowns cleanly.

## Managing parameters with `DacMsSqlParameters`

```csharp
using Microsoft.Data.SqlClient;
using am.kon.packages.dac.mssql;

var parameters = new DacMsSqlParameters()
    .AddItem("@CustomerId", 178)
    .AddItem("@IsActive", true);

// Migrate from older helpers by projecting to SqlParameter[]
SqlParameter[] legacy = parameters.ToArray();
```

`DacMsSqlParameters` also accepts `SqlParameter` instances, `KeyValuePair<string, object>` sequences, and reflection-based input via `AddRange` helpers.

## Reading data with `ExecuteReaderAsync`

```csharp
await using var reader = await database.ExecuteReaderAsync(
    sql: "dbo.GetCustomer",
    parameters: parameters.ToArray(),
    commandType: CommandType.StoredProcedure);

while (await reader.ReadAsync())
{
    Console.WriteLine(reader.GetString(reader.GetOrdinal("DisplayName")));
}
```

Use the overloads that accept `SqlParameter[]`, `IDataParameter[]`, or `DacMsSqlParameters` to match your calling code. When the stored procedure sets a non-zero return value, a `DacSqlExecutionReturnedErrorCodeException` is thrown with the return code and the open reader attached.

## Executing non-query commands

```csharp
int affected = await database.ExecuteNonQueryAsync(
    sql: "UPDATE Customers SET IsActive = 0 WHERE CustomerId = @CustomerId",
    parameters: parameters.ToArray());
```

Every overload ultimately delegates to the same implementation, so you can pass `IDataParameter[]`, `SqlParameter[]`, `DacSqlParameters` (legacy, marked obsolete), or `DacMsSqlParameters`.

## Executing scalar commands

```csharp
object orderCount = await database.ExecuteScalarAsync(
    sql: "SELECT COUNT(1) FROM Sales.Orders WHERE CustomerId = @CustomerId",
    parameters: parameters.ToArray());
```

The result is the value of the first column of the first row. Use the `throwDBException`, `throwGenericException`, and `throwSystemException` flags to tailor exception propagation.

## Filling existing containers

`FillData<T>` populates an existing `DataTable` or `DataSet`. You can cap the returned rows with `startRecord`/`maxRecords` when you need paging.

```csharp
var table = new DataTable();
database.FillData(
    dataOut: table,
    sql: "SELECT * FROM Sales.Orders WHERE CustomerId = @CustomerId",
    parameters: parameters.ToArray(),
    startRecord: 0,
    maxRecords: 100);
```

`FillDataTable` and `FillDataSet` are convenience overloads that forward to `FillData<T>` for the respective container types.

## Returning new `DataTable` / `DataSet` instances

```csharp
DataTable table = database.GetDataTable(
    sql: "SELECT TOP (100) * FROM Sales.Customers ORDER BY CreatedAt DESC",
    parameters: parameters.ToArray());

DataSet report = database.GetDataSet(
    sql: "dbo.GetCustomerSummary",
    parameters: parameters.ToArray(),
    commandType: CommandType.StoredProcedure,
    startRecord: 0,
    maxRecords: 0);
```

Both methods internally allocate the container, call `FillData<T>`, then return the populated instance.

## Executing batches with an open connection

Use `ExecuteSQLBatchAsync` when you need a single open connection for multiple commands but do not require an explicit transaction.

```csharp
using Microsoft.Data.SqlClient;

var totals = await database.ExecuteSQLBatchAsync(async connection =>
{
    using var countCustomers = new SqlCommand("SELECT COUNT(1) FROM Sales.Customers", (SqlConnection)connection);
    int customerCount = (int)await countCustomers.ExecuteScalarAsync();

    using var countOrders = new SqlCommand("SELECT COUNT(1) FROM Sales.Orders", (SqlConnection)connection);
    int orderCount = (int)await countOrders.ExecuteScalarAsync();

    return (Customers: customerCount, Orders: orderCount);
});
```

The delegate receives the open `IDbConnection`. Set `closeConnection` to `false` when you need to manage the lifetime yourself.

## Executing transactional batches

`ExecuteTransactionalSQLBatchAsync` wraps your delegate in a transaction and commits automatically when the delegate completes successfully.

```csharp
using Microsoft.Data.SqlClient;

await database.ExecuteTransactionalSQLBatchAsync(async transaction =>
{
    var conn = (SqlConnection)transaction.Connection;
    var sqlTransaction = (SqlTransaction)transaction;

    using var debit = new SqlCommand("UPDATE Accounts SET Balance = Balance - @Amount WHERE AccountId = @Source", conn, sqlTransaction);
    debit.Parameters.AddRange(new[]
    {
        new SqlParameter("@Amount", amount),
        new SqlParameter("@Source", sourceAccountId)
    });
    await debit.ExecuteNonQueryAsync();

    using var credit = new SqlCommand("UPDATE Accounts SET Balance = Balance + @Amount WHERE AccountId = @Target", conn, sqlTransaction);
    credit.Parameters.AddRange(new[]
    {
        new SqlParameter("@Amount", amount),
        new SqlParameter("@Target", targetAccountId)
    });
    await credit.ExecuteNonQueryAsync();

    return Task.CompletedTask;
});
```

Throwing a `DacSqlExecutionReturnedErrorCodeException`, any other `DacGenericException`, or a regular exception inside the delegate triggers a rollback. You can toggle the `throw*Exception` flags to suppress and handle errors yourself.

## Extending `DataBase`

Create derived classes when you want to bundle domain-specific helpers or enforce consistent parameter creation.

```csharp
public sealed class ReportingDataBase : DataBase
{
    public ReportingDataBase(string connectionString, CancellationToken token)
        : base(connectionString, token) { }

    public Task<object> GetActiveCustomerCountAsync()
    {
        var parameters = new DacMsSqlParameters()
            .AddItem("@IsActive", true);

        return ExecuteScalarAsync(
            sql: "SELECT COUNT(1) FROM Sales.Customers WHERE IsActive = @IsActive",
            parameters: parameters.ToArray());
    }

    public Task<int> ArchiveCustomerAsync(int customerId)
    {
        var parameters = new DacMsSqlParameters()
            .AddItem("@CustomerId", customerId);

        return ExecuteNonQueryAsync(
            sql: "dbo.Customer_Archive",
            parameters: parameters.ToArray(),
            commandType: CommandType.StoredProcedure);
    }
}
```

Because all public members are virtual through the `IDataBase` interface, derived classes can add new overloads or wrap existing ones without re-implementing connection management.

## Handling return codes and exceptions

Every command adds a `@return_value` parameter automatically. When the command assigns a non-zero value, a `DacSqlExecutionReturnedErrorCodeException` is thrown with the code and the returned object (reader, dataset, scalar value, etc.). Toggle the `throwDBException`, `throwGenericException`, and `throwSystemException` arguments to intercept particular categories of failures without throwing.

## Cancellation and cleanup

- Pass a scoped `CancellationToken` into the constructor to cancel long-running queries.
- The library closes connections automatically unless you pass `closeConnection: false` to the batch helpers.
- Wrapping readers in `await using` ensures the connection is released even when you suppress exceptions.

For higher-level service integration and configuration-backed connection management, see [`am.kon.packages.services.dac.mssql`](../am.kon.packages.services.dac.mssql/README.md).
