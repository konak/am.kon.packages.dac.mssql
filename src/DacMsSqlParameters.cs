using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using am.kon.packages.dac.common.Cache;
using Microsoft.Data.SqlClient;

namespace am.kon.packages.dac.mssql;

/// <summary>
/// Represents a collection of SQL parameters with methods for adding,
/// managing, and enumerating the parameters.
/// </summary>
public class DacMsSqlParameters : IEnumerable<SqlParameter>
{
    /// <summary>
    /// Represents a private list of SQL parameters used internally to store and manage the collection of parameters
    /// associated with the current instance of the DacMsSqlParameters class.
    /// </summary>
    private readonly List<SqlParameter> _parameters;
    
    /// <summary>
    /// Initializes a new instance of the DacMsSqlParameters class with an empty collection of SQL parameters.
    /// </summary>
    public DacMsSqlParameters()
    {
        _parameters = new();
    }
    
    /// <summary>
    /// Initializes a new instance of the DacMsSqlParameters class with the specified initial capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity of the SQL parameters collection.</param>
    public DacMsSqlParameters(int capacity)
    {
        _parameters = new(capacity);
    }
    
    /// <summary>
    /// Initializes a new instance of the DacMsSqlParameters class with the specified collection of SQL parameters.
    /// </summary>
    /// <param name="collection">The collection of SQL parameters to initialize with.</param>
    public DacMsSqlParameters(IEnumerable<SqlParameter> collection)
    {
        _parameters = new(collection);
    }

    /// <summary>
    /// Adds a single SQL parameter to the current instance using the specified parameter name and value.
    /// </summary>
    /// <param name="name">The name of the SQL parameter to be added.</param>
    /// <param name="value">The value of the SQL parameter to be added.</param>
    /// <returns>The current instance with the newly added parameter.</returns>
    public DacMsSqlParameters AddItem(string name, object value)
    {
        _parameters.Add(new SqlParameter(name, value ?? DBNull.Value));;
        return this;
    }

    /// <summary>
    /// Adds a single SQL parameter to the current instance using the specified SqlParameter object.
    /// </summary>
    /// <param name="item">The SqlParameter object to be added to the collection.</param>
    /// <returns>The current instance with the newly added parameter.</returns>
    public DacMsSqlParameters AddItem(SqlParameter item)
    {
        _parameters.Add(item);
        return this;
    }

    /// <summary>
    /// Adds a single SQL parameter to the current instance using the specified parameter name and value.
    /// </summary>
    /// <param name="name">The name of the SQL parameter to be added.</param>
    /// <param name="value">The value of the SQL parameter to be added.</param>
    /// <returns>The current instance with the newly added parameter.</returns>
    public DacMsSqlParameters AddItem(KeyValuePair<string, object> item)
    {
        _parameters.Add(new SqlParameter(item.Key, item.Value ?? DBNull.Value));;
        return this;
    }

    /// <summary>
    /// Adds a collection of SQL parameters to the current instance from an enumerable collection of SqlParameter objects.
    /// </summary>
    /// <param name="collection">An enumerable collection of SqlParameter objects to be added to the current instance.</param>
    /// <returns>The current instance with the added parameters.</returns>
    public DacMsSqlParameters AddRange(IEnumerable<SqlParameter> collection)
    {
        _parameters.AddRange(collection);
        return this;
    }

    /// <summary>
    /// Adds a collection of SQL parameters to the current instance from another instance of DacMsSqlParameters.
    /// </summary>
    /// <param name="collection">An instance of DacMsSqlParameters containing the SQL parameters to be added.</param>
    /// <returns>The current instance with the added parameters.</returns>
    public DacMsSqlParameters AddRange(DacMsSqlParameters collection)
    {
        _parameters.AddRange(collection);
        return this;
    }

    /// <summary>
    /// Adds a range of SQL parameters to the collection from an enumerable collection of key-value pairs.
    /// </summary>
    /// <param name="collection">An enumerable collection of key-value pairs where the key represents the parameter name and the value is the parameter value.</param>
    /// <returns>The current instance with the added parameters.</returns>
    public DacMsSqlParameters AddRange(IEnumerable<KeyValuePair<string, object>> collection)
    {
        foreach (KeyValuePair<string, object> record in collection)
        {
            _parameters.Add(new SqlParameter(record.Key, record.Value ?? DBNull.Value));;
        }
        
        return this;
    }

    /// <summary>
    /// Reads property values from a dynamically typed object and adds them to the collection of SQL parameters.
    /// </summary>
    /// <param name="parameters">A dynamically typed object containing property values to be read.</param>
    /// <returns>The current instance with the added parameters.</returns>
    private DacMsSqlParameters ReadFromDynamic(dynamic parameters)
    {
        if (parameters == null)
            return this;
        
        PropertyInfo[] properties = parameters.GetType().GetProperties();
        
        return ReadFromProperties(properties, parameters);
    }

    /// <summary>
    /// Reads property values from a generic object and adds them to the collection of SQL parameters.
    /// </summary>
    /// <typeparam name="T">The type of the object containing property values.</typeparam>
    /// <param name="parameters">The object containing the property values to be read.</param>
    /// <returns>The current instance with the added parameters.</returns>
    private DacMsSqlParameters ReadFromGeneric<T>(T parameters)
    {
        if (parameters == null)
            return this;

        PropertyInfo[] properties = PropertyInfoCache.GetProperties(typeof(T));
        
        return ReadFromProperties(properties, parameters);
    }

    /// <summary>
    /// Reads property values from the specified object and adds them to the collection of SQL parameters.
    /// </summary>
    /// <param name="properties">An array of properties to extract values from.</param>
    /// <param name="parameters">The object containing the property values to be read.</param>
    /// <returns>The current instance with the added parameters.</returns>
    private DacMsSqlParameters ReadFromProperties(PropertyInfo[] properties, object parameters)
    {
        if (properties == null || properties.Length == 0 || parameters == null)
            return this;

        foreach (PropertyInfo propertyInfo in properties)
        {
            object val = propertyInfo.GetValue(parameters);
            _parameters.Add(new SqlParameter(propertyInfo.Name, val ?? DBNull.Value));
        }

        return this;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection of SQL parameters.
    /// </summary>
    /// <returns>An enumerator for the collection.</returns>
    public IEnumerator<SqlParameter> GetEnumerator()
    {
        return _parameters.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection of SQL parameters.
    /// </summary>
    /// <returns>An enumerator for the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public SqlParameter[] ToArray()
    {
        return _parameters.ToArray();
    }
}