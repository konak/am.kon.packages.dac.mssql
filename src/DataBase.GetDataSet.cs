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
    /// Retrieves a new DataSet for the specified SQL command or stored procedure with parameters and execution options.
    /// </summary>
    /// <param name="sql">The SQL command text, stored procedure name, or table name.</param>
    /// <param name="parameters">The parameters of the SQL command.</param>
    /// <param name="commandType">The type of SQL command to execute (Text, StoredProcedure, or TableDirect).</param>
    /// <param name="throwDBException">Indicates whether to throw database-specific exceptions during SQL execution.</param>
    /// <param name="throwGenericException">Indicates whether to throw general exceptions encountered during execution.</param>
    /// <param name="throwSystemException">Indicates whether to throw system-level exceptions encountered during execution.</param>
    /// <param name="startRecord">The zero-based index of the first record to retrieve.</param>
    /// <param name="maxRecords">The maximum number of records to retrieve.</param>
    /// <returns>A new DataSet containing the results of the SQL command execution.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public DataSet GetDataSet(string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0,
        int maxRecords = 0)
    {
        DataSet ds = new DataSet();

        FillData<DataSet>(ds, sql, parameters, commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);

        return ds;
    }

    /// <summary>
    /// Retrieves a DataSet for the specified SQL command or stored procedure with parameters and execution options.
    /// </summary>
    /// <param name="sql">The SQL command text, stored procedure name, or table name.</param>
    /// <param name="parameters">The parameters associated with the SQL command.</param>
    /// <param name="commandType">Specifies how the SQL command is interpreted (Text, StoredProcedure, or TableDirect).</param>
    /// <param name="throwDBException">Indicates whether to throw database-specific exceptions during execution.</param>
    /// <param name="throwGenericException">Indicates whether to throw general exceptions encountered during execution.</param>
    /// <param name="throwSystemException">Indicates whether to throw system-level exceptions encountered during execution.</param>
    /// <param name="startRecord">The zero-based index of the first record to retrieve.</param>
    /// <param name="maxRecords">The maximum number of records to retrieve.</param>
    /// <returns>A DataSet containing the results of the SQL execution.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public DataSet GetDataSet(string sql, SqlParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0,
        int maxRecords = 0)
    {
        DataSet ds = new DataSet();

        FillData<DataSet>(ds, sql, parameters, commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);

        return ds;
    }

    /// <summary>
    /// Retrieves a DataSet based on the provided SQL command, parameters, and execution options.
    /// </summary>
    /// <param name="sql">The SQL command text, stored procedure name, or table name.</param>
    /// <param name="parameters">The parameters to be passed to the SQL command, represented as DacSqlParameters.</param>
    /// <param name="commandType">The type of SQL command to execute (Text, StoredProcedure, or TableDirect).</param>
    /// <param name="throwDBException">Indicates whether database-specific exceptions should be thrown during SQL execution.</param>
    /// <param name="throwGenericException">Indicates whether general exceptions encountered during execution should be thrown.</param>
    /// <param name="throwSystemException">Indicates whether to throw system-level exceptions encountered during execution.</param>
    /// <param name="startRecord">The zero-based index of the first record to retrieve.</param>
    /// <param name="maxRecords">The maximum number of records to retrieve. If set to 0, retrieves all records.</param>
    /// <returns>A DataSet containing the result of the SQL command execution.</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    [Obsolete("Use DataSet GetDataSet(string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0) instead.", false)]
    public DataSet GetDataSet(string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0,
        int maxRecords = 0)
    {
        DataSet ds = new DataSet();

        FillData<DataSet>(ds, sql, parameters.ToDataParameters(), commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);

        return ds;
    }

    /// <summary>
    /// Get new dataset for specified SQL command or stored procedure
    /// </summary>
    /// <param name="sql">SQL command text, stored procedure or table name</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <param name="throwDBException">Throw SQL execution exceptions or suspend them</param>
    /// <param name="throwGenericException">Throw Generic exceptions or suspend them</param>
    /// <param name="throwSystemException">Throw System exceptions or suspend them</param>
    /// <param name="startRecord">The zero based record number to start with</param>
    /// <param name="maxRecords">The maximum number of records to retrive</param>
    /// <returns>New DataSet of results of SQL command</returns>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public DataSet GetDataSet(string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        DataSet ds = new DataSet();

        FillData<DataSet>(ds, sql, (IDataParameter[])parameters.ToArray(), commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);

        return ds;
    }

}

