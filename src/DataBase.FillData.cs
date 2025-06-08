using System;
using System.Data;
using am.kon.packages.dac.mssql.Extensions;
using am.kon.packages.dac.primitives;
using am.kon.packages.dac.primitives.Constants.Exception;
using am.kon.packages.dac.primitives.Exceptions;
using Microsoft.Data.SqlClient;

namespace am.kon.packages.dac.mssql;

public partial class DataBase
{
    /// <summary>
    /// Executes the specified SQL command or stored procedure and fills the provided data object with the retrieved results.
    /// </summary>
    /// <typeparam name="T">The type of the object to populate with the data. Supported types include DataTable and DataSet.</typeparam>
    /// <param name="dataOut">The object to populate with the data retrieved from the database.</param>
    /// <param name="sql">The SQL command, stored procedure name, or table name to execute or query.</param>
    /// <param name="parameters">The parameters to pass to the SQL command.</param>
    /// <param name="commandType">The type of the SQL command being executed, such as Text or StoredProcedure. Default is Text.</param>
    /// <param name="throwDBException">Indicates whether database-specific exceptions should be thrown. Default is true.</param>
    /// <param name="throwGenericException">Indicates whether generic exceptions should be thrown. Default is true.</param>
    /// <param name="throwSystemException">Indicates whether system-level exceptions should be thrown. Default is true.</param>
    /// <param name="startRecord">The zero-based record number from which to start retrieving data. Default is 0.</param>
    /// <param name="maxRecords">The maximum number of records to retrieve. Default is 0, which retrieves all records.</param>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public void FillData<T>(T dataOut, string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true,
        int startRecord = 0, int maxRecords = 0)
    {
        SqlCommand cmd = null;
        SqlDataAdapter da = null;

        try
        {
            cmd = new SqlCommand(sql, new SqlConnection(this._connectionString));
            cmd.CommandType = commandType;

            SqlParameter rv = new SqlParameter("@return_value", SqlDbType.Int);
            rv.Direction = ParameterDirection.ReturnValue;

            cmd.Parameters.Add(rv);

            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            da = new SqlDataAdapter(cmd);

            switch (dataOut)
            {
                case DataTable:
                    if (maxRecords == 0)
                        da.Fill(dataOut as DataTable);
                    else
                        da.Fill(startRecord, maxRecords, new DataTable[] { dataOut as DataTable });

                    break;

                case DataSet:
                    if (maxRecords == 0)
                        da.Fill(dataOut as DataSet);
                    else
                        da.Fill(dataOut as DataSet, startRecord, maxRecords, string.Empty);

                    break;

                default:
                    if (throwSystemException)
                        throw new DacGenericException(Messages.FILL_DATA_INVALID_TYPE_PASSED + typeof(T).ToString());
                    break;
            }

            int retVal = (int)rv.Value;

            if (retVal != 0)
                throw new DacSqlExecutionReturnedErrorCodeException(retVal, dataOut);
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
                if (cmd != null)
                    cmd.Connection?.Close();
            }
            catch (Exception ex)
            {
                if (throwSystemException)
                    throw new DacGenericException(Messages.SQL_CONNECTION_CLOSE_EXCEPTION, ex);
            }
        }
    }

    /// <summary>
    /// Executes the specified SQL command or stored procedure and fills the provided data object with the retrieved results.
    /// </summary>
    /// <typeparam name="T">The type of the object to populate with the data. Supported types include DataTable and DataSet.</typeparam>
    /// <param name="dataOut">The object to populate with the data retrieved from the database.</param>
    /// <param name="sql">The SQL command or stored procedure name to execute.</param>
    /// <param name="parameters">An array of parameters to pass to the SQL command.</param>
    /// <param name="commandType">The type of the SQL command being executed, such as Text or StoredProcedure. Default is Text.</param>
    /// <param name="throwDBException">Indicates whether database-specific exceptions should be thrown. Default is true.</param>
    /// <param name="throwGenericException">Indicates whether generic exceptions should be thrown. Default is true.</param>
    /// <param name="throwSystemException">Indicates whether system-level exceptions should be thrown. Default is true.</param>
    /// <param name="startRecord">The zero-based record number from which to start retrieving data. Default is 0.</param>
    /// <param name="maxRecords">The maximum number of records to retrieve. Default is 0, which retrieves all records.</param>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public void FillData<T>(T dataOut, string sql, SqlParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true,
        int startRecord = 0, int maxRecords = 0)
    {
        FillData(dataOut, sql, (IDataParameter[])parameters, commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    /// <summary>
    /// Executes the provided SQL command or stored procedure and fills the output object with the retrieved results.
    /// </summary>
    /// <typeparam name="T">The type of the object to populate with the retrieved data. Supported types include DataTable and DataSet.</typeparam>
    /// <param name="dataOut">The object to populate with the data retrieved from the database.</param>
    /// <param name="sql">The SQL command or stored procedure to execute.</param>
    /// <param name="parameters">The parameters to include in the SQL command.</param>
    /// <param name="commandType">Specifies the type of the command to execute, either Text or StoredProcedure. Default is Text.</param>
    /// <param name="throwDBException">Indicates if database-specific exceptions should be thrown. Default is true.</param>
    /// <param name="throwGenericException">Indicates if generic exceptions should be thrown. Default is true.</param>
    /// <param name="throwSystemException">Indicates if system-level exceptions should be thrown. Default is true.</param>
    /// <param name="startRecord">The zero-based index of the first record to retrieve. Default is 0.</param>
    /// <param name="maxRecords">The maximum number of records to retrieve. Default is 0, which retrieves all records.</param>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public void FillData<T>(T dataOut, string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true,
        int startRecord = 0,
        int maxRecords = 0)
    {
        FillData(dataOut, sql, parameters.ToDataParameters(), commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    /// <summary>
    /// Executes the specified SQL command or stored procedure and fills the provided data object with the retrieved results.
    /// </summary>
    /// <typeparam name="T">The type of the object to populate with the data. Supported types include DataTable and DataSet.</typeparam>
    /// <param name="dataOut">The object to populate with the data retrieved from the database.</param>
    /// <param name="sql">The SQL command, stored procedure name, or table name to execute or query.</param>
    /// <param name="parameters">The collection of SQL parameters to pass to the SQL command, encapsulated in a <see cref="DacMsSqlParameters"/> object.</param>
    /// <param name="commandType">The type of the SQL command being executed, such as Text or StoredProcedure. Default is Text.</param>
    /// <param name="throwDBException">Indicates whether database-specific exceptions should be thrown. Default is true.</param>
    /// <param name="throwGenericException">Indicates whether generic exceptions should be thrown. Default is true.</param>
    /// <param name="throwSystemException">Indicates whether system-level exceptions should be thrown. Default is true.</param>
    /// <param name="startRecord">The zero-based record number from which to start retrieving data. Default is 0.</param>
    /// <param name="maxRecords">The maximum number of records to retrieve. Default is 0, which retrieves all records.</param>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public void FillData<T>(T dataOut, string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true,
        int startRecord = 0, int maxRecords = 0)
    {
        FillData(dataOut, sql, (IDataParameter[])parameters.ToArray(), commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }
}

