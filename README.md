# am.kon.packages.dac.mssql

`am.kon.packages.dac.mssql` is a thin data-access helper around `Microsoft.Data.SqlClient` that keeps the API surface aligned with the abstractions in `am.kon.packages.dac.primitives`. It provides convenience methods for executing ad-hoc SQL, filling `DataTable`/`DataSet` instances, and working with strongly typed parameter collections.

## Installation

```bash
 dotnet add package am.kon.packages.dac.mssql
```

## Quick start

```csharp
using System.Data;
using System.Threading;
using am.kon.packages.dac.mssql;

var cts = new CancellationTokenSource();
var database = new DataBase(
    connectionString: "Server=localhost;Database=Sample;Trusted_Connection=True;",
    cancellationToken: cts.Token);

// Build a parameter bag once and reuse it across calls
var parameters = new DacMsSqlParameters()
    .AddItem("@CustomerId", 178)
    .AddItem("@IsActive", true);

// Execute a reader – remember to dispose the IDataReader when done
await using var reader = await database.ExecuteReaderAsync(
    sql: "dbo.GetCustomer",
    parameters: parameters.ToArray(),
    commandType: CommandType.StoredProcedure);

while (await reader.ReadAsync())
{
    Console.WriteLine(reader["DisplayName"]);
}
```

## Executing commands

```csharp
// Non-query commands return the affected row count
int updated = await database.ExecuteNonQueryAsync(
    sql: "UPDATE Customers SET IsActive = 0 WHERE CustomerId = @CustomerId",
    parameters: parameters.ToArray());

// Scalar commands return the first column of the first row
object total = await database.ExecuteScalarAsync(
    sql: "SELECT COUNT(1) FROM Orders WHERE CustomerId = @CustomerId",
    parameters: parameters.ToArray());
```

## Working with DataTable / DataSet

```csharp
var table = new DataTable();
database.FillData(
    dataOut: table,
    sql: "SELECT * FROM Orders WHERE CustomerId = @CustomerId",
    parameters: parameters.ToArray());

var dataSet = database.GetDataSet(
    sql: "dbo.GetCustomerSummary",
    parameters: parameters.ToArray(),
    commandType: CommandType.StoredProcedure);
```

## Handling return codes and exceptions

All methods support handling SQL return codes by throwing `DacSqlExecutionReturnedErrorCodeException` when the server sets a non-zero return value. You can opt out of re-throwing SQL or generic exceptions by toggling the `throwDBException`, `throwGenericException`, and `throwSystemException` flags on each call when you need custom error handling.

Pass a cancellation token via the constructor and call `CancellationTokenSource.Cancel()` when you want to stop long-running operations gracefully.
