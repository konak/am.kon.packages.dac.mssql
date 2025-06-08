using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using am.kon.packages.dac.primitives;
using am.kon.packages.dac.primitives.Constants.Exception;
using am.kon.packages.dac.primitives.Exceptions;
using Microsoft.Data.SqlClient;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("am.kon.packages.dac.mssql")]

namespace am.kon.packages.dac.mssql;

/// <summary>
/// Represents a relational database abstraction that implements functionality for executing SQL commands and transactions.
/// </summary>
public partial class DataBase : IDataBase
{
    // private readonly Type _dataTableType = typeof(DataTable);
    // private readonly Type _dataSetType = typeof(DataSet);
    private readonly string _connectionString;
    private readonly CancellationToken _cancellationToken;

    /// <summary>
    /// Connection string of <see cref="IDataBase"/> connection
    /// </summary>
    public string ConnectionString { get { return _connectionString; } }

    /// <summary>
    /// Initializes a new instance of the DataBase class.
    /// </summary>
    /// <param name="connectionString">The connection string used to establish database connection.</param>
    /// <param name="cancellationToken">The cancellation token to cancel async operations.</param>
    public DataBase(string connectionString, CancellationToken cancellationToken)
    {
        _connectionString = connectionString;
        _cancellationToken = cancellationToken;
    }

    /// <summary>
    /// Executes a batch of SQL commands asynchronously within a single database connection.
    /// </summary>
    /// <typeparam name="T">The type of the result returned by the batch execution.</typeparam>
    /// <param name="batch">A function that encapsulates the logic for executing the batch of SQL commands.</param>
    /// <param name="closeConnection">Specifies whether the database connection should be closed after execution.</param>
    /// <param name="throwDbException">Indicates whether a database-related exception should be thrown if an error occurs.</param>
    /// <param name="throwGenericException">Indicates whether a generic exception should be thrown if an error occurs.</param>
    /// <param name="throwSystemException">Indicates whether a system exception should be thrown if an error occurs.</param>
    /// <returns>A task representing the asynchronous operation, with the result of type <typeparamref name="T"/>.</returns>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when @return_value parameter becomes a nonzero value, indication error code returned by query or stored procedure.</exception>
    /// <exception cref="DacSqlExecutionException">Thrown when a SQL-related error occurs.</exception>
    /// <exception cref="DacGenericException">Thrown when a generic error occurs.</exception>
    public async Task<T> ExecuteSQLBatchAsync<T>(Func<IDbConnection, Task<T>> batch, bool closeConnection = true, bool throwDbException = true, bool throwGenericException = true, bool throwSystemException = true)
    {
        T res = default;
        SqlConnection connection = null;

        try
        {
            connection = new SqlConnection(this._connectionString);
            await connection.OpenAsync(_cancellationToken);
            res = await batch(connection);
        }
        catch (SqlException ex)
        {
            if (throwDbException)
                throw new DacSqlExecutionException(ex);
        }
        catch (DacSqlExecutionReturnedErrorCodeException)
        {
            throw;
        }
        catch (DacGenericException)
        {
            if (throwGenericException)
                throw;
        }
        catch (Exception ex)
        {
            if (throwSystemException)
                throw new DacGenericException(Messages.SYSTEM_EXCEPTION_ON_EXECUTE_SQL_BATCH_LEVEL, ex);
        }
        finally
        {
            try
            {
                if (closeConnection && connection != null)
                    await connection.CloseAsync();
            }
            catch (Exception ex)
            {
                if (throwSystemException)
                    throw new DacGenericException(Messages.SQL_CONNECTION_CLOSE_EXCEPTION, ex);
            }
        }

        return res;
    }

    /// <summary>
    /// Executes a batch of SQL statements within a transaction asynchronously.
    /// </summary>
    /// <param name="batch">The delegate function representing the batch to be executed, which receives an <see cref="IDbTransaction"/> instance.</param>
    /// <param name="closeConnection">Indicates whether the database connection should be closed after execution. Default is true.</param>
    /// <param name="throwDBException">Indicates whether database-related exceptions should be thrown. Default is true.</param>
    /// <param name="throwGenericException">Indicates whether generic data access exceptions should be thrown. Default is true.</param>
    /// <param name="throwSystemException">Indicates whether system exceptions should be rethrown as a custom wrapped exception. Default is true.</param>
    /// <typeparam name="T">The type of the object returned by the batch function.</typeparam>
    /// <returns>A task that represents the asynchronous operation. The task result contains the object of type <typeparamref name="T"/> returned by the batch function.</returns>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when @return_value parameter becomes a nonzero value, indication error code returned by query or stored procedure.</exception>
    /// <exception cref="DacSqlExecutionException">Thrown when a SQL-related error occurs.</exception>
    /// <exception cref="DacGenericException">Thrown when a generic error occurs.</exception>
    public async Task<T> ExecuteTransactionalSQLBatchAsync<T>(Func<IDbTransaction, Task<T>> batch, bool closeConnection = true, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true)
    {
        T res = default;
        SqlConnection connection = null;
        DbTransaction transaction = null;

        try
        {
            connection = new SqlConnection(this._connectionString);

            await connection.OpenAsync(_cancellationToken);
            transaction = await connection.BeginTransactionAsync(_cancellationToken);
            res = await batch(transaction);
            await transaction.CommitAsync(_cancellationToken);
        }
        catch (SqlException ex)
        {
            if (transaction != null)
                await transaction.RollbackAsync(_cancellationToken);

            if (throwDBException)
                throw new DacSqlExecutionException(ex);
        }
        catch (DacSqlExecutionReturnedErrorCodeException)
        {
            throw;
        }
        catch (DacGenericException)
        {
            if (transaction != null)
                await transaction.RollbackAsync(_cancellationToken);

            if (throwGenericException)
                throw;
        }
        catch (Exception ex)
        {
            if (transaction != null)
                await transaction.RollbackAsync(_cancellationToken);

            if (throwSystemException)
                throw new DacGenericException(Messages.SYSTEM_EXCEPTION_ON_EXECUTE_SQL_BATCH_LEVEL, ex);
        }
        finally
        {
            try
            {
                if (closeConnection && connection != null)
                    await connection.CloseAsync();
            }
            catch (Exception ex)
            {
                if (throwSystemException)
                    throw new DacGenericException(Messages.SQL_CONNECTION_CLOSE_EXCEPTION, ex);
            }
        }

        return res;
    }

}

