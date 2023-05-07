using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using am.kon.packages.dac.mssql.Extensions;
using am.kon.packages.dac.primitives;
using am.kon.packages.dac.primitives.Exceptions;

namespace am.kon.packages.dac.mssql;

public partial class DataBase : IDataBase
{
    /// <summary>
    /// Private Method to execute SQL command or stored procedure and return <see cref="SqlDataReader"/> SqlDataReader object to read data 
    /// </summary>
    /// <param name="sql">SQL command, stored procedure or table name</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <returns>Data reader object to read data</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
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

        SqlDataReader res = await sqlCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);

        int retVal = 0;

        if (returnValue.Value != null)
            retVal = (int)returnValue.Value;

        if (retVal != 0)
            throw new DacSqlExecutionReturnedErrorCodeException(retVal, res);

        return res;
    }

    /// <summary>
    /// Execute SQL command asyncronously and return <see cref="SqlDataReader"/> SqlDataReader object to read data
    /// </summary>
    /// <param name="sql">SQL command, stored procedure or table name</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <returns>Data reader object to read data</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<IDataReader> ExecuteReaderAsync(string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true)
    {
        Func<IDbConnection, Task<IDataReader>> executeReaderAsyncFunction = connection => ExecuteReaderAsyncInternal(connection, sql, parameters, commandType);

        return ExecuteSQLBatchAsync<IDataReader>(executeReaderAsyncFunction, false, throwDBException, throwGenericException, throwSystemException);
    }

    /// <summary>
    /// Execute SQL command asyncronously and return <see cref="SqlDataReader"/> SqlDataReader object to read data
    /// </summary>
    /// <param name="sql">SQL command, stored procedure or table name</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <returns>Data reader object to read data</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<IDataReader> ExecuteReaderAsync(string sql, KeyValuePair<string, object>[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true)
    {
        Func<IDbConnection, Task<IDataReader>> executeReaderAsyncFunction = connection => ExecuteReaderAsyncInternal(connection, sql, parameters.ToDataParameters(), commandType);

        return ExecuteSQLBatchAsync<IDataReader>(executeReaderAsyncFunction, false, throwDBException, throwGenericException, throwSystemException);
    }

    /// <summary>
    /// Execute SQL command asyncronously and return <see cref="SqlDataReader"/> SqlDataReader object to read data
    /// </summary>
    /// <param name="sql">SQL command, stored procedure or table name</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <returns>Data reader object to read data</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<IDataReader> ExecuteReaderAsync(string sql, List<KeyValuePair<string, object>> parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true)
    {
        Func<IDbConnection, Task<IDataReader>> executeReaderAsyncFunction = connection => ExecuteReaderAsyncInternal(connection, sql, parameters.ToDataParameters(), commandType);

        return ExecuteSQLBatchAsync<IDataReader>(executeReaderAsyncFunction, false, throwDBException, throwGenericException, throwSystemException);
    }

    /// <summary>
    /// Execute SQL command asyncronously and return <see cref="SqlDataReader"/> SqlDataReader object to read data
    /// </summary>
    /// <param name="sql">SQL command, stored procedure or table name</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <returns>Data reader object to read data</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<IDataReader> ExecuteReaderAsync(string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true)
    {
        Func<IDbConnection, Task<IDataReader>> executeReaderAsyncFunction = connection => ExecuteReaderAsyncInternal(connection, sql, parameters.ToDataParameters(), commandType);

        return ExecuteSQLBatchAsync<IDataReader>(executeReaderAsyncFunction, false, throwDBException, throwGenericException, throwSystemException);
    }

    /// <summary>
    /// Execute SQL command asyncronously and return <see cref="SqlDataReader"/> SqlDataReader object to read data
    /// </summary>
    /// <param name="sql">SQL command, stored procedure or table name</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <returns>Data reader object to read data</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<IDataReader> ExecuteReaderAsync(string sql, dynamic parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true)
    {
        Func<IDbConnection, Task<IDataReader>> executeReaderAsyncFunction = connection => ExecuteReaderAsyncInternal(connection, sql, parameters.ToDataParameters(), commandType);

        return ExecuteSQLBatchAsync<IDataReader>(executeReaderAsyncFunction, false, throwDBException, throwGenericException, throwSystemException);
    }

    /// <summary>
    /// Execute SQL command asyncronously and return <see cref="SqlDataReader"/> SqlDataReader object to read data
    /// </summary>
    /// <param name="sql">SQL command, stored procedure or table name</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <returns>Data reader object to read data</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<IDataReader> ExecuteReaderAsync<T>(string sql, T parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true)
    {
        Func<IDbConnection, Task<IDataReader>> executeReaderAsyncFunction = connection => ExecuteReaderAsyncInternal(connection, sql, parameters.ToDataParameters(), commandType);

        return ExecuteSQLBatchAsync<IDataReader>(executeReaderAsyncFunction, false, throwDBException, throwGenericException, throwSystemException);
    }
}

