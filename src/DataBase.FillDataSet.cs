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
    /// Fills the specified DataSet object with data obtained from executing a SQL command with provided parameters.
    /// </summary>
    /// <param name="ds">The DataSet instance to be populated with retrieved data.</param>
    /// <param name="sql">The SQL query or command text to execute.</param>
    /// <param name="parameters">An array of IDataParameter objects to supply parameters to the SQL command.</param>
    /// <param name="commandType">Specifies how the SQL command is interpreted, such as Text for raw queries or StoredProcedure. Default is CommandType.Text.</param>
    /// <param name="throwDBException">Specifies whether database-related exceptions should be thrown when errors occur. Default is true.</param>
    /// <param name="throwGenericException">Specifies whether generic exceptions should be thrown when errors occur. Default is true.</param>
    /// <param name="throwSystemException">Specifies whether system-level exceptions should be thrown when errors occur. Default is true.</param>
    /// <param name="startRecord">The zero-based index indicating the starting point for retrieving data. Default is 0.</param>
    /// <param name="maxRecords">The maximum number of records to retrieve. Default is 0, which means no record limit.</param>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public void FillDataSet(DataSet ds, string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true,
        int startRecord = 0, int maxRecords = 0)
    {
        FillData<DataSet>(ds, sql, parameters, commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    /// <summary>
    /// Fills the provided DataSet object with data retrieved from the execution of a SQL command.
    /// </summary>
    /// <param name="ds">The DataSet object that will be populated with the retrieved data.</param>
    /// <param name="sql">The SQL query or command text to be executed.</param>
    /// <param name="parameters">An array of SqlParameter objects containing parameter values to be used with the SQL query.</param>
    /// <param name="commandType">Defines how the SQL command string should be interpreted (e.g., Text or StoredProcedure). Default is CommandType.Text.</param>
    /// <param name="throwDBException">Indicates whether to throw a database-specific exception if an error occurs. Default is true.</param>
    /// <param name="throwGenericException">Indicates whether to throw a generic exception if an error occurs. Default is true.</param>
    /// <param name="throwSystemException">Indicates whether to throw a system-level exception if an error occurs. Default is true.</param>
    /// <param name="startRecord">The zero-based record number to start retrieving data from. Default is 0.</param>
    /// <param name="maxRecords">The maximum number of records to retrieve. Default is 0, which means no limit.</param>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Thrown when the SQL query or stored procedure returns a non-zero error code.</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public void FillDataSet(DataSet ds, string sql, SqlParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true,
        int startRecord = 0, int maxRecords = 0)
    {
        FillData<DataSet>(ds, sql, parameters, commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    /// <summary>
    /// Fill provided DataSet item with values from executed SQL command
    /// </summary>
    /// <param name="ds">A DatSet item that must be filled with data</param>
    /// <param name="sql">SQL command text to be executed</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <param name="startRecord">The zero based record number to start with</param>
    /// <param name="maxRecords">The maximum number of records to retrive</param>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    [Obsolete("Use FillDataSet(DataSet ds, string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0) instead.", false)]
    public void FillDataSet(DataSet ds, string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        FillData<DataSet>(ds, sql, parameters.ToDataParameters(), commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    /// <summary>
    /// Fill provided DataSet item with values from executed SQL command
    /// </summary>
    /// <param name="ds">A DatSet item that must be filled with data</param>
    /// <param name="sql">SQL command text to be executed</param>
    /// <param name="commandType">SQL command type to execute</param>
    /// <param name="parameters">Parameters of the SQL command</param>
    /// <param name="startRecord">The zero based record number to start with</param>
    /// <param name="maxRecords">The maximum number of records to retrive</param>
    /// <exception cref="DacSqlExecutionException">Throws if any SqlException has accured</exception>
    /// <exception cref="DacSqlExecutionReturnedErrorCodeException">Throws if SQL query or stored procedure has returned non zero code</exception>
    /// <exception cref="DacGenericException">Throws if any Generic exception has accured</exception>
    public void FillDataSet(DataSet ds, string sql, DacMsSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        FillData<DataSet>(ds, sql, (IDataParameter[])parameters.ToArray(), commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }
}

