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
    /// Executes a SQL command asynchronously and retrieves the value of the first column of the first row from the result set.
    /// </summary>
    /// <param name="sql">The SQL command text to be executed.</param>
    /// <param name="parameters">The parameters to pass to the SQL command.</param>
    /// <param name="commandType">The type of the SQL command, such as Text, StoredProcedure, or TableDirect. Default is Text.</param>
    /// <returns>The value of the first column of the first row from the result set.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<object> ExecuteScalarAsync(string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text)
    {
        Func<IDbConnection, Task<object>> b = async delegate(IDbConnection connection)
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
    /// Executes a SQL command asynchronously and retrieves the value of the first column of the first row from the result set.
    /// </summary>
    /// <param name="sql">The SQL command text to be executed.</param>
    /// <param name="parameters">The array of SqlParameter objects to pass to the SQL command.</param>
    /// <param name="commandType">The type of the SQL command, such as Text, StoredProcedure, or TableDirect. Default is Text.</param>
    /// <returns>The value of the first column of the first row from the result set.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<object> ExecuteScalarAsync(string sql, SqlParameter[] parameters, CommandType commandType = CommandType.Text)
    {
        return ExecuteScalarAsync(sql, (IDataParameter[])parameters, commandType);
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
    [Obsolete("Use ExecuteScalarAsync<T>(string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text) instead.", false)]
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
    public Task<object> ExecuteScalarAsync(string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text)
    {
        return ExecuteScalarAsync(sql, parameters.ToArray(), commandType);
    }
}

