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
    /// Executes a SQL non-query and returns the number of affected rows.
    /// </summary>
    /// <param name="sql">The SQL command text to execute.</param>
    /// <param name="parameters">The parameters of the SQL command.</param>
    /// <param name="commandType">The type of the SQL command.</param>
    /// <returns>The number of affected rows.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<int> ExecuteNonQueryAsync(string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text)
    {
        Func<IDbConnection, Task<int>> func = async delegate(IDbConnection connection)
        {
            SqlConnection conn = connection as SqlConnection;
            SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.CommandType = commandType;

            SqlParameter rv = new SqlParameter("@return_value", SqlDbType.Int);
            rv.Direction = ParameterDirection.ReturnValue;

            cmd.Parameters.Add(rv);

            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            int res = await cmd.ExecuteNonQueryAsync(_cancellationToken);

            int retVal = 0;
            if (rv.Value != null && rv.Value != DBNull.Value)
                retVal = Convert.ToInt32(rv.Value);

            if (retVal != 0)
                throw new DacSqlExecutionReturnedErrorCodeException(retVal, res);

            return res;
        };

        return ExecuteSQLBatchAsync<int>(func);
    }

    /// <summary>
    /// Executes a SQL non-query and returns the number of affected rows.
    /// </summary>
    /// <param name="sql">The SQL command text to execute.</param>
    /// <param name="parameters">The parameters of the SQL command.</param>
    /// <param name="commandType">The type of the SQL command.</param>
    /// <returns>The number of affected rows.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<int> ExecuteNonQueryAsync(string sql, SqlParameter[] parameters, CommandType commandType = CommandType.Text)
    {
        return ExecuteNonQueryAsync(sql, (IDataParameter[])parameters, commandType);
    }

    /// <summary>
    /// Executes a SQL non-query and returns the number of affected rows.
    /// </summary>
    /// <param name="sql">The SQL command text to execute.</param>
    /// <param name="parameters">The parameters of the SQL command provided as an instance of DacSqlParameters.</param>
    /// <param name="commandType">The type of the SQL command.</param>
    /// <returns>The number of affected rows.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    [Obsolete("Use ExecuteNonQueryAsync<T>(string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text) instead", false)]
    public Task<int> ExecuteNonQueryAsync(string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text)
    {
        return ExecuteNonQueryAsync(sql, parameters.ToDataParameters(), commandType);
    }

    /// <summary>
    /// Executes a SQL non-query and returns the number of affected rows.
    /// </summary>
    /// <typeparam name="T">The generic type parameter for the method.</typeparam>
    /// <param name="sql">The SQL command text to execute.</param>
    /// <param name="parameters">The collection of SQL parameters to be used.</param>
    /// <param name="commandType">The type of the SQL command.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the number of affected rows.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public Task<int> ExecuteNonQueryAsync(string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text)
    {
        return ExecuteNonQueryAsync(sql, (IDataParameter[])parameters.ToArray(), commandType);
    }
}
