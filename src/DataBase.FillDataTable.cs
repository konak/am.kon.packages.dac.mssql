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
    /// Fills the provided DataTable with data retrieved from the execution of the specified SQL command.
    /// </summary>
    /// <param name="dt">The DataTable to populate with data.</param>
    /// <param name="sql">The SQL command text to execute.</param>
    /// <param name="parameters">The collection of SQL parameters used for the command execution.</param>
    /// <param name="commandType">Specifies the type of SQL command, such as Text, StoredProcedure, etc. Defaults to Text.</param>
    /// <param name="throwDBException">Indicates whether to throw a database-specific exception when an error occurs during execution. Defaults to true.</param>
    /// <param name="throwGenericException">Indicates whether to throw a generic exception when an error occurs during execution. Defaults to true.</param>
    /// <param name="throwSystemException">Indicates whether to throw a system-level exception when an error occurs during execution. Defaults to true.</param>
    /// <param name="startRecord">The zero-based index of the first record to begin fetching. Defaults to 0.</param>
    /// <param name="maxRecords">The maximum number of records to fetch. Defaults to 0 (fetch all records).</param>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public void FillDataTable(DataTable dt, string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true,
        int startRecord = 0, int maxRecords = 0)
    {
        FillData<DataTable>(dt, sql, parameters, commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    /// <summary>
    /// Fills the provided DataTable with data retrieved from the SQL command execution.
    /// </summary>
    /// <param name="dt">The DataTable to populate with data.</param>
    /// <param name="sql">The SQL command text to execute.</param>
    /// <param name="parameters">The collection of SQL parameters to use with the SQL command.</param>
    /// <param name="commandType">Specifies the type of SQL command, such as Text or StoredProcedure.</param>
    /// <param name="throwDBException">Indicates whether to throw a database-specific exception on errors.</param>
    /// <param name="throwGenericException">Indicates whether to throw a generic exception on errors.</param>
    /// <param name="throwSystemException">Indicates whether to throw a system-level exception on errors.</param>
    /// <param name="startRecord">The zero-based record index to begin retrieval.</param>
    /// <param name="maxRecords">The maximum number of records to fetch.</param>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public void FillDataTable(DataTable dt, string sql, SqlParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true,
        int startRecord = 0, int maxRecords = 0)
    {
        FillData<DataTable>(dt, sql, parameters, commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    /// <summary>
    /// Fills the provided DataTable with data retrieved from the execution of a SQL command.
    /// </summary>
    /// <param name="dt">The DataTable to populate with the data retrieved from the SQL command.</param>
    /// <param name="sql">The SQL command text to execute for retrieving the data.</param>
    /// <param name="parameters">The collection of DacSqlParameters to use as input parameters for the SQL command.</param>
    /// <param name="commandType">Specifies the type of the SQL command, such as Text or StoredProcedure. Default is CommandType.Text.</param>
    /// <param name="throwDBException">Determines whether to throw a database-specific exception (e.g., SqlException) if an error occurs during execution. Default is true.</param>
    /// <param name="throwGenericException">Determines whether to throw a generic exception in case of errors during execution. Default is true.</param>
    /// <param name="throwSystemException">Determines whether to throw a system-level exception in case of unexpected errors. Default is true.</param>
    /// <param name="startRecord">The zero-based index of the first record to retrieve. Default is 0.</param>
    /// <param name="maxRecords">The maximum number of records to return. A value of 0 means no limit. Default is 0.</param>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    [Obsolete("Use FillDataTable(DataTable dt, string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0) instead.", false)]
    public void FillDataTable(DataTable dt, string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true,
        int startRecord = 0, int maxRecords = 0)
    {
        FillData<DataTable>(dt, sql, parameters.ToDataParameters(), commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    /// <summary>
    /// Fills the provided DataTable with data retrieved from the SQL command execution.
    /// </summary>
    /// <param name="dt">The DataTable to populate with data.</param>
    /// <param name="sql">The SQL command text to execute.</param>
    /// <param name="parameters">The DacMsSqlParameters collection of SQL parameters to use with the SQL command.</param>
    /// <param name="commandType">Specifies the type of SQL command, such as Text or StoredProcedure.</param>
    /// <param name="throwDBException">Indicates whether to throw a database-specific exception on errors.</param>
    /// <param name="throwGenericException">Indicates whether to throw a generic exception on errors.</param>
    /// <param name="throwSystemException">Indicates whether to throw a system-level exception on errors.</param>
    /// <param name="startRecord">The zero-based record index to begin retrieval.</param>
    /// <param name="maxRecords">The maximum number of records to fetch.</param>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has occurred.</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has occurred.</exception>
    public void FillDataTable(DataTable dt, string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        FillData<DataTable>(dt, sql, (IDataParameter[])parameters.ToArray(), commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

}

