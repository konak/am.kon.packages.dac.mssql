using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using am.kon.packages.dac.primitives;
using am.kon.packages.dac.primitives.Constants.Exception;
using am.kon.packages.dac.primitives.Exceptions;

namespace am.kon.packages.dac.mssql;

public partial class DataBase : IDataBase
{
    private readonly Type _dataTableType = typeof(DataTable);
    private readonly Type _dataSetType = typeof(DataSet);
    private readonly string _connectionString;
    private readonly CancellationToken _cancellationToken;

    /// <summary>
    /// Connection string of <see cref="IDataBase"/> connection
    /// </summary>
    public string ConnectionString { get { return _connectionString; } }

    public DataBase(string connectionString, CancellationToken cancellationToken)
    {
        _connectionString = connectionString;
        _cancellationToken = cancellationToken;
    }

    /// <summary>
    /// Async version of <see cref="ExecuteSQLBatch"/> to execute SQL batch job
    /// </summary>
    /// <typeparam name="T">A generic type of object the batch must return</typeparam>
    /// <param name="batch">SQL batch job object</param>
    /// <param name="throwDBException">Throw SQL execution exceptions or suspend them</param>
    /// <param name="throwGenericException">Throw Generic exceptions or suspend them</param>
    /// <param name="throwSystemException">Throw System exceptions or suspend them</param>
    /// <param name="closeConnection">Close connection after batch job execution</param>
    /// <returns>Batch execution result object</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public async Task<T> ExecuteSQLBatchAsync<T>(Func<IDbConnection, Task<T>> batch, bool closeConnection = true, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true)
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
            if (throwDBException)
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
    /// Async version of <see cref="ExecuteTransactionalSQLBatch"/> to execute transactional SQL batch job
    /// </summary>
    /// <typeparam name="T">A generic type of object the batch must return</typeparam>
    /// <param name="batch">A transactional SQL batch job object</param>
    /// <param name="throwDBException">Throw SQL execution exceptions or suspend them</param>
    /// <param name="throwGenericException">Throw Generic exceptions or suspend them</param>
    /// <param name="throwSystemException">Throw System exceptions or suspend them</param>
    /// <param name="closeConnection">Close connection after batch job execution</param>
    /// <returns>Batch execution result object</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
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

