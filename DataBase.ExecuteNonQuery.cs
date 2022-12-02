using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using am.kon.packages.dac.mssql.Extensions;
using am.kon.packages.dac.primitives;
using am.kon.packages.dac.primitives.Exceptions;

namespace am.kon.packages.dac.mssql
{
    public partial class DataBase : IDataBase
    {
        /// <summary>
        /// Execute SQL query and return the number of affected values
        /// </summary>
        /// <param name="sql">SQL command text to be executed</param>
        /// <param name="commandType">SQL command type to execute</param>
        /// <param name="parameters">Parameters of the SQL command</param>
        /// <returns>Number of affected rows</returns>
        /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
        /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
        /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
        public Task<int> ExecuteNonQueryAsync(string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text)
        {
            Func<IDbConnection, Task<int>> func = async delegate (IDbConnection connection)
            {
                SqlConnection conn = connection as SqlConnection;
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.CommandType = commandType;

                SqlParameter rv = new SqlParameter("@return_value", SqlDbType.Int);
                rv.Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.Add(rv);

                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                int res = await cmd.ExecuteNonQueryAsync();

                int retVal = (int)rv.Value;

                if (retVal != 0) throw new DacSqlExecutionReturnedErrorCodeException(retVal, res);

                return res;
            };

            return ExecuteSQLBatchAsync<int>(func);
        }

        /// <summary>
        /// Execute SQL query and return the number of affected values
        /// </summary>
        /// <param name="sql">SQL command text to be executed</param>
        /// <param name="commandType">SQL command type to execute</param>
        /// <param name="parameters">Parameters of the SQL command</param>
        /// <returns>Number of affected rows</returns>
        /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
        /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
        /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
        public Task<int> ExecuteNonQueryAsync(string sql, KeyValuePair<string, object>[] parameters, CommandType commandType = CommandType.Text)
        {
            return ExecuteNonQueryAsync(sql, parameters.ToDataParameters(), commandType);
        }

        /// <summary>
        /// Execute SQL query and return the number of affected values
        /// </summary>
        /// <param name="sql">SQL command text to be executed</param>
        /// <param name="commandType">SQL command type to execute</param>
        /// <param name="parameters">Parameters of the SQL command</param>
        /// <returns>Number of affected rows</returns>
        /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
        /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
        /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
        public Task<int> ExecuteNonQueryAsync(string sql, List<KeyValuePair<string, object>> parameters, CommandType commandType = CommandType.Text)
        {
            return ExecuteNonQueryAsync(sql, parameters.ToDataParameters(), commandType);
        }

        /// <summary>
        /// Execute SQL query and return the number of affected values
        /// </summary>
        /// <param name="sql">SQL command text to be executed</param>
        /// <param name="commandType">SQL command type to execute</param>
        /// <param name="parameters">Parameters of the SQL command</param>
        /// <returns>Number of affected rows</returns>
        /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
        /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
        /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
        public Task<int> ExecuteNonQueryAsync(string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text)
        {
            return ExecuteNonQueryAsync(sql, parameters.ToDataParameters(), commandType);
        }

        /// <summary>
        /// Execute SQL query and return the number of affected values
        /// </summary>
        /// <param name="sql">SQL command text to be executed</param>
        /// <param name="commandType">SQL command type to execute</param>
        /// <param name="parameters">Parameters of the SQL command</param>
        /// <returns>Number of affected rows</returns>
        /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
        /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
        /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
        public Task<int> ExecuteNonQueryAsync(string sql, dynamic parameters, CommandType commandType = CommandType.Text)
        {
            return ExecuteNonQueryAsync(sql, parameters.ToDataParameters(), commandType);
        }

        /// <summary>
        /// Execute SQL query and return the number of affected values
        /// </summary>
        /// <param name="sql">SQL command text to be executed</param>
        /// <param name="commandType">SQL command type to execute</param>
        /// <param name="parameters">Parameters of the SQL command</param>
        /// <returns>Number of affected rows</returns>
        /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
        /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
        /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
        public Task<int> ExecuteNonQueryAsync<T>(string sql, T parameters, CommandType commandType = CommandType.Text)
        {
            return ExecuteNonQueryAsync(sql, parameters.ToDataParameters(), commandType);
        }

    }
}

