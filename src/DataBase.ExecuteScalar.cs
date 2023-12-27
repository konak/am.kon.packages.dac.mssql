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
    /// Execute SQL command and return value of first column of the first row from results
    /// </summary>
    /// <param name="sql">SQL command text to be executed</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <returns>Value of first column of the first row</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<object> ExecuteScalarAsync(string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text)
    {
        Func<IDbConnection, Task<object>> b = async delegate (IDbConnection connection)
        {
            SqlConnection conn = connection as SqlConnection;
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.CommandType = commandType;

            SqlParameter rv = new SqlParameter("@return_value", SqlDbType.Int);
            rv.Direction = ParameterDirection.ReturnValue;

            cmd.Parameters.Add(rv);

            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            object res = await cmd.ExecuteScalarAsync(_cancellationToken);

            int retVal = (int)rv.Value;

            if (retVal != 0)
                throw new DacSqlExecutionReturnedErrorCodeException(retVal, res);

            return res;
        };

        return ExecuteSQLBatchAsync<object>(b);
    }

    /// <summary>
    /// Execute SQL command and return value of first column of the first row from results
    /// </summary>
    /// <param name="sql">SQL command text to be executed</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <returns>Value of first column of the first row</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<object> ExecuteScalarAsync(string sql, SqlParameter[] parameters, CommandType commandType = CommandType.Text)
    {
        return ExecuteScalarAsync(sql, parameters.ToDataParameters(), commandType);
    }

    /// <summary>
    /// Execute SQL command and return value of first column of the first row from results
    /// </summary>
    /// <param name="sql">SQL command text to be executed</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <returns>Value of first column of the first row</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<object> ExecuteScalarAsync(string sql, KeyValuePair<string, object>[] parameters, CommandType commandType = CommandType.Text)
    {
        return ExecuteScalarAsync(sql, parameters.ToDataParameters(), commandType);
    }

    /// <summary>
    /// Execute SQL command and return value of first column of the first row from results
    /// </summary>
    /// <param name="sql">SQL command text to be executed</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <returns>Value of first column of the first row</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<object> ExecuteScalarAsync(string sql, List<KeyValuePair<string, object>> parameters, CommandType commandType = CommandType.Text)
    {
        return ExecuteScalarAsync(sql, parameters.ToDataParameters(), commandType);
    }

    /// <summary>
    /// Execute SQL command and return value of first column of the first row from results
    /// </summary>
    /// <param name="sql">SQL command text to be executed</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <returns>Value of first column of the first row</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<object> ExecuteScalarAsync(string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text)
    {
        return ExecuteScalarAsync(sql, parameters.ToDataParameters(), commandType);
    }

    /// <summary>
    /// Execute SQL command and return value of first column of the first row from results
    /// </summary>
    /// <param name="sql">SQL command text to be executed</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <returns>Value of first column of the first row</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<object> ExecuteScalarAsync(string sql, dynamic parameters, CommandType commandType = CommandType.Text)
    {
        return ExecuteScalarAsync(sql, parameters.ToDataParameters(), commandType);
    }

    /// <summary>
    /// Execute SQL command and return value of first column of the first row from results
    /// </summary>
    /// <param name="sql">SQL command text to be executed</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <returns>Value of first column of the first row</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<object> ExecuteScalarAsync<T>(string sql, T parameters, CommandType commandType = CommandType.Text)
    {
        return ExecuteScalarAsync(sql, parameters.ToDataParameters(), commandType);
    }
}

