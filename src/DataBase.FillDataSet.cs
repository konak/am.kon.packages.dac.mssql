using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using am.kon.packages.dac.mssql.Extensions;
using am.kon.packages.dac.primitives;
using am.kon.packages.dac.primitives.Exceptions;

namespace am.kon.packages.dac.mssql;

public partial class DataBase : IDataBase
{
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
    public void FillDataSet(DataSet ds, string sql, SqlParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
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
    public void FillDataSet(DataSet ds, string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
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
    public void FillDataSet(DataSet ds, string sql, KeyValuePair<string, object>[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
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
    public void FillDataSet(DataSet ds, string sql, List<KeyValuePair<string, object>> parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
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
    public void FillDataSet(DataSet ds, string sql, dynamic parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
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
    public void FillDataSet<T>(DataSet ds, string sql, T parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        FillData<DataSet>(ds, sql, parameters.ToDataParameters(), commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }
}

