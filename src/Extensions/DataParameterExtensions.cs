﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using am.kon.packages.dac.primitives;
using Microsoft.Data.SqlClient;

namespace am.kon.packages.dac.mssql.Extensions
{
    public static class DataParameterExtensions
    {
        /// <summary>
        /// Extension method to transform <see cref="KeyValuePair<string, object>[]"/> instance to an array of <see cref="IDataParameter"/>
        /// </summary>
        /// <param name="parameters">instance the extension is applyed to</param>
        /// <returns></returns>
        private static IDataParameter[] ToDataParameters(this KeyValuePair<string, object>[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return [];
        
            int len = parameters.Length;
            SqlParameter[] sqlParameters = new SqlParameter[len];
        
            KeyValuePair<string, object> param;
        
            for (int i = 0; i < len; i++)
            {
                param = parameters[i];
                sqlParameters[i] = new SqlParameter(param.Key, param.Value ?? DBNull.Value);
            }
        
            return sqlParameters;
        }

        /// <summary>
        /// Extension method to transform <see cref="DacSqlParameters"/> instance to an array of <see cref="IDataParameter"/>
        /// </summary>
        /// <param name="parameters">instance the extension is applyed to</param>
        /// <returns></returns>
        public static IDataParameter[] ToDataParameters(this DacSqlParameters parameters)
        {
            return ToDataParameters(parameters.ToArray());
        }
        
        // /// <summary>
        // /// Extension method to transform <see cref="List<KeyValuePair<string, object>>"/> instance to an array of <see cref="IDataParameter"/>
        // /// </summary>
        // /// <param name="parameters">instance the extension is applyed to</param>
        // /// <returns></returns>
        // public static IDataParameter[] ToDataParameters(this List<KeyValuePair<string, object>> parameters)
        // {
        //     return ToDataParameters(parameters.ToArray());
        // }

        /// <summary>
        /// Get <see cref="SqlParameter[]"/> based on provided <see cref="PropertyInfo[]"/>
        /// </summary>
        /// <param name="properties">Array of <see cref="PropertyInfo"/> to generate array of <see cref="SqlParameter"/>.</param>
        /// <param name="parameters">Ann ojbect containinng parameters</param>
        /// <returns></returns>
        private static IDataParameter[] PropertyInfoToSqlParameters(PropertyInfo[] properties, object parameters)
        {
            if (properties == null || properties.Length == 0 || parameters == null)
                return Array.Empty<IDataParameter>();

            SqlParameter[] sqlParameters = new SqlParameter[properties.Length];

            PropertyInfo propInfo;

            for (int i = 0, len = properties.Length; i < len; i++)
            {
                propInfo = properties[i];
                object val = propInfo.GetValue(parameters);
                sqlParameters[i] = new SqlParameter(propInfo.Name, val ?? DBNull.Value);
            }

            return sqlParameters;
        }

        // /// <summary>
        // /// Extension method to transform <see cref="dynamic"/> instance to an array of <see cref="IDataParameter"/>
        // /// </summary>
        // /// <param name="parameters">instance the extension is applyed to</param>
        // /// <returns></returns>
        // public static IDataParameter[] ToDataParameters(dynamic parameters)
        // {
        //     if (parameters == null)
        //         return Array.Empty<IDataParameter>();
        //
        //     PropertyInfo[] properties = parameters.GetType().GetProperties();
        //
        //     return PropertyInfoToSqlParameters(properties, parameters);
        // }

        // /// <summary>
        // /// Extension method to transform generic type instance to an array of <see cref="IDataParameter"/>
        // /// </summary>
        // /// <typeparam name="T">Generic  type of the instance</typeparam>
        // /// <param name="parameters">item containing properties interpreted as parameters</param>
        // /// <returns></returns>
        // public static IDataParameter[] ToDataParameters<T>(this T parameters)
        // {
        //     if (parameters == null)
        //         return Array.Empty<IDataParameter>();
        //
        //     PropertyInfo[] properties = PropertyInfoCache.GetProperties(typeof(T));
        //
        //     return PropertyInfoToSqlParameters(properties, parameters);
        // }
    }
}

