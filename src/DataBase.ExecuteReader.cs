using System;
using System.Data;
using System.Threading.Tasks;
using am.kon.packages.dac.mssql.Extensions;
using am.kon.packages.dac.primitives;
using am.kon.packages.dac.primitives.Exceptions;
using Microsoft.Data.SqlClient;

namespace am.kon.packages.dac.mssql;

public partial class DataBase
{
    /// <summary>
    /// Private Method to execute SQL command or stored procedure and return <see cref="SqlDataReader"/> SqlDataReader object to read data 
    /// </summary>
    /// <param name="connection">Open SQL connection used to execute the command.</param>
    /// <param name="sqlQuery">SQL command, stored procedure or table name.</param>
    /// <param name="parameters">Parameters of the SQL command.</param>
    /// <param name="commandType">SQL command type to execute.</param>
    /// <returns>Data reader object to read data.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    internal async Task<IDataReader> ExecuteReaderAsyncInternal(IDbConnection connection, string sqlQuery, IDataParameter[] parameters, CommandType commandType = CommandType.Text)
    {
        SqlConnection conn = connection as SqlConnection;
        SqlCommand sqlCommand = new SqlCommand(sqlQuery, conn);

        sqlCommand.CommandType = commandType;

        SqlParameter returnValue = new SqlParameter("@return_value", SqlDbType.Int);
        returnValue.Direction = ParameterDirection.ReturnValue;
        returnValue.IsNullable = false;

        sqlCommand.Parameters.Add(returnValue);

        if (parameters != null && parameters.Length > 0)
            sqlCommand.Parameters.AddRange(parameters);

        SqlDataReader res = await sqlCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection, _cancellationToken);

        int retVal = 0;

        if (returnValue.Value != null && returnValue.Value != DBNull.Value)
            retVal = Convert.ToInt32(returnValue.Value);

        if (retVal != 0)
            throw new DacSqlExecutionReturnedErrorCodeException(retVal, res);

        return res;
    }

    /// <summary>
    /// Executes an SQL query or stored procedure asynchronously and returns an <see cref="IDataReader"/> object for reading the resulting data.
    /// </summary>
    /// <param name="sql">The SQL query or stored procedure to be executed.</param>
    /// <param name="parameters">The parameters to be applied to the SQL query or stored procedure.</param>
    /// <param name="commandType">Indicates whether the SQL query is a text command or a stored procedure. Default is CommandType.Text.</param>
    /// <param name="throwDBException">Indicates whether to throw database-specific exceptions. Default is true.</param>
    /// <param name="throwGenericException">Indicates whether to throw generic exceptions. Default is true.</param>
    /// <param name="throwSystemException">Indicates whether to throw general system exceptions. Default is true.</param>
    /// <returns>An <see cref="IDataReader"/> object for reading the results of the query.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<IDataReader> ExecuteReaderAsync(string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true)
    {
        Func<IDbConnection, Task<IDataReader>> executeReaderAsyncFunction = connection => ExecuteReaderAsyncInternal(connection, sql, parameters, commandType);

        return ExecuteSQLBatchAsync<IDataReader>(executeReaderAsyncFunction, false, throwDBException, throwGenericException, throwSystemException);
    }

    /// <summary>
    /// Executes a SQL command or stored procedure asynchronously and returns an <see cref="IDataReader"/> to read data.
    /// </summary>
    /// <param name="sql">The SQL command or stored procedure to execute.</param>
    /// <param name="parameters">The array of SQL parameters to be passed to the command.</param>
    /// <param name="commandType">The type of the SQL command, such as Text or StoredProcedure. Default is Text.</param>
    /// <param name="throwDBException">Determines whether database-specific exceptions (e.g., SqlException) should be thrown. Default is true.</param>
    /// <param name="throwGenericException">Determines whether generic exceptions should be thrown. Default is true.</param>
    /// <param name="throwSystemException">Determines whether system-level exceptions should be thrown. Default is true.</param>
    /// <returns>An asynchronous task that resolves to an <see cref="IDataReader"/> object to read the result set.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<IDataReader> ExecuteReaderAsync(string sql, SqlParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true)
    {
        Func<IDbConnection, Task<IDataReader>> executeReaderAsyncFunction = connection => ExecuteReaderAsyncInternal(connection, sql, parameters, commandType);

        return ExecuteSQLBatchAsync<IDataReader>(executeReaderAsyncFunction, false, throwDBException, throwGenericException, throwSystemException);
    }
    
    /// <summary>
    /// Executes the specified SQL query or stored procedure and returns a data reader object to read the resulting data.
    /// </summary>
    /// <param name="sql">The SQL query or stored procedure to execute.</param>
    /// <param name="parameters">The parameters to pass to the SQL query or stored procedure.</param>
    /// <param name="commandType">Specifies the type of the command being executed (Text, StoredProcedure, etc.).</param>
    /// <param name="throwDBException">Indicates whether to throw a database-specific exception if an error occurs.</param>
    /// <param name="throwGenericException">Indicates whether to throw a generic exception if an error occurs.</param>
    /// <param name="throwSystemException">Indicates whether to throw a system-level exception if an error occurs.</param>
    /// <returns>A task that represents the asynchronous operation, which upon completion contains a data reader object to read the query results.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    [Obsolete("Use ExecuteReaderAsync<T>(string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true) instead", false)]
    public Task<IDataReader> ExecuteReaderAsync(string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true)
    {
        Func<IDbConnection, Task<IDataReader>> executeReaderAsyncFunction = connection => ExecuteReaderAsyncInternal(connection, sql, parameters.ToDataParameters(), commandType);

        return ExecuteSQLBatchAsync<IDataReader>(executeReaderAsyncFunction, false, throwDBException, throwGenericException, throwSystemException);
    }

    /// <summary>
    /// Executes a SQL command or stored procedure asynchronously and returns an <see cref="IDataReader"/> for reading data.
    /// </summary>
    /// <param name="sql">The SQL command or stored procedure to execute.</param>
    /// <param name="parameters">The SQL parameters to include with the command.</param>
    /// <param name="commandType">The type of SQL command, such as Text or StoredProcedure.</param>
    /// <param name="throwDBException">Determines whether database-specific exceptions should be thrown in case of an error.</param>
    /// <param name="throwGenericException">Determines whether generic exceptions should be thrown in case of an error.</param>
    /// <param name="throwSystemException">Determines whether system-level exceptions should be thrown in case of an error.</param>
    /// <returns>A task representing the asynchronous operation, with a result of <see cref="IDataReader"/> to read data.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<IDataReader> ExecuteReaderAsync(string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true)
    {
        Func<IDbConnection, Task<IDataReader>> executeReaderAsyncFunction = connection => ExecuteReaderAsyncInternal(connection, sql, (IDataParameter[])parameters.ToArray(), commandType);

        return ExecuteSQLBatchAsync<IDataReader>(executeReaderAsyncFunction, false, throwDBException, throwGenericException, throwSystemException);
    }
}
