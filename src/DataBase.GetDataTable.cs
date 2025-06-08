using System;
using System.Data;
using am.kon.packages.dac.mssql.Extensions;
using am.kon.packages.dac.primitives;
using am.kon.packages.dac.primitives.Exceptions;
using Microsoft.Data.SqlClient;

namespace am.kon.packages.dac.mssql;

public partial class DataBase
{
    /// <summary>
    /// Retrieves a new DataTable for a specified SQL command.
    /// </summary>
    /// <param name="sql">The SQL command text to be executed.</param>
    /// <param name="parameters">The parameters to be passed to the SQL command.</param>
    /// <param name="commandType">The type of the SQL command to execute. Default is CommandType.Text.</param>
    /// <param name="throwDBException">Specifies whether to throw SQL execution exceptions or suspend them. Default is true.</param>
    /// <param name="throwGenericException">Specifies whether to throw generic exceptions or suspend them. Default is true.</param>
    /// <param name="throwSystemException">Specifies whether to throw system exceptions or suspend them. Default is true.</param>
    /// <param name="startRecord">The zero-based record number to start with. Default is 0.</param>
    /// <param name="maxRecords">The maximum number of records to retrieve. Default is 0.</param>
    /// <returns>A DataTable populated with the results of the SQL command.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public DataTable GetDataTable(string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0,
        int maxRecords = 0)
    {
        DataTable dt = new DataTable("Table0");

        FillData<DataTable>(dt, sql, parameters, commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);

        return dt;
    }

    /// <summary>
    /// Retrieves a DataTable populated with the results of the SQL command executed.
    /// </summary>
    /// <param name="sql">The SQL command text to be executed.</param>
    /// <param name="parameters">An array of SQL parameters to be passed to the SQL command.</param>
    /// <param name="commandType">The type of the SQL command to execute. Default is CommandType.Text.</param>
    /// <param name="throwDBException">Indicates whether to throw exceptions related to SQL execution errors. Default is true.</param>
    /// <param name="throwGenericException">Indicates whether to throw generic exceptions during execution. Default is true.</param>
    /// <param name="throwSystemException">Indicates whether to throw system-level exceptions. Default is true.</param>
    /// <param name="startRecord">The zero-based record number to start retrieving data from. Default is 0.</param>
    /// <param name="maxRecords">The maximum number of records to retrieve. Default is 0.</param>
    /// <returns>A DataTable containing the results of the SQL command.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public DataTable GetDataTable(string sql, SqlParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0,
        int maxRecords = 0)
    {
        DataTable dt = new DataTable("Table0");

        FillData<DataTable>(dt, sql, parameters, commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);

        return dt;
    }

    /// <summary>
    /// Retrieves a DataTable based on a specified SQL command and parameters.
    /// </summary>
    /// <param name="sql">The SQL command text to be executed.</param>
    /// <param name="parameters">The DacSqlParameters containing the parameters for the SQL command.</param>
    /// <param name="commandType">The type of SQL command to execute. Default is CommandType.Text.</param>
    /// <param name="throwDBException">Specifies whether to throw database-related exceptions. Default is true.</param>
    /// <param name="throwGenericException">Specifies whether to throw generic exceptions. Default is true.</param>
    /// <param name="throwSystemException">Specifies whether to throw system-related exceptions. Default is true.</param>
    /// <param name="startRecord">The zero-based record number to start retrieving from. Default is 0.</param>
    /// <param name="maxRecords">The maximum number of records to retrieve. Default is 0.</param>
    /// <returns>A DataTable containing the results of the executed SQL command.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    [Obsolete("Use GetDataTable(string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0) instead.", false)]
    public DataTable GetDataTable(string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0,
        int maxRecords = 0)
    {
        DataTable dt = new DataTable("Table0");

        FillData<DataTable>(dt, sql, parameters.ToDataParameters(), commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);

        return dt;
    }

    /// <summary>
    /// Get new DataTable for specified sql command
    /// </summary>
    /// <param name="sql">SQL command text to be executed</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <param name="throwDBException">Throw SQL execution exceptions or suspend them</param>
    /// <param name="throwGenericException">Throw Generic exceptions or suspend them</param>
    /// <param name="throwSystemException">Throw System exceptions or suspend them</param>
    /// <param name="startRecord">The zero based record number to start with</param>
    /// <param name="maxRecords">The maximum number of records to retrive</param>
    /// <returns>New DataTable item with results of the SQL command</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public DataTable GetDataTable(string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        DataTable dt = new DataTable("Table0");

        FillData<DataTable>(dt, sql, (IDataParameter[])parameters.ToArray(), commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);

        return dt;
    }

}

