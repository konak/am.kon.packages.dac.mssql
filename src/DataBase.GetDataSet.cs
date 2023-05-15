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
    public DataSet GetDataSet(string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        DataSet ds = new DataSet();

        FillData<DataSet>(ds, sql, parameters, commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);

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
    public DataSet GetDataSet(string sql, SqlParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        DataSet ds = new DataSet();

        FillData<DataSet>(ds, sql, parameters, commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);

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
    public DataSet GetDataSet(string sql, KeyValuePair<string, object>[] parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
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
    public DataSet GetDataSet(string sql, List<KeyValuePair<string, object>> parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
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
    public DataSet GetDataSet(string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
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
    public DataSet GetDataSet(string sql, dynamic parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
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
    public DataSet GetDataSet<T>(string sql, T parameters, CommandType commandType = CommandType.Text, bool throwDBException = true, bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        DataSet ds = new DataSet();

        FillData<DataSet>(ds, sql, parameters.ToDataParameters(), commandType, throwDBException, throwGenericException, throwSystemException, startRecord, maxRecords);

        return ds;
    }
}

