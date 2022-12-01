using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using am.kon.packages.dac.common.Cache;
using am.kon.packages.dac.primitives;

namespace am.kon.packages.dac.mssql.Extensions
{
    public static class DataParameterExtensions
    {
        /// <summary>
        /// Extension methid to transform <see cref="KeyValuePair<string, object>[]"/> instance to an array of <see cref="IDataParameter"/>
        /// </summary>
        /// <param name="parameters">instance the extension is applyed to</param>
        /// <returns></returns>
        public static IDataParameter[] ToDataParameters(this KeyValuePair<string, object>[] parameters)
        {
            SqlParameter[] sqlParameters = null;
            int len = parameters.Length;

            if (parameters != null && len > 0)
            {
                sqlParameters = new SqlParameter[len];

                KeyValuePair<string, Object> param;

                for (int i = 0; i < len; i++)
                {
                    param = parameters[i];
                    sqlParameters[i] = new SqlParameter(param.Key, param.Value == null ? DBNull.Value : param.Value);
                }
            }

            return sqlParameters;
        }

        /// <summary>
        /// Extension methid to transform <see cref="DacSqlParameters"/> instance to an array of <see cref="IDataParameter"/>
        /// </summary>
        /// <param name="parameters">instance the extension is applyed to</param>
        /// <returns></returns>
        public static IDataParameter[] ToDataParameters(this DacSqlParameters parameters)
        {
            return ToDataParameters(parameters.ToArray());
        }

        /// <summary>
        /// Extension methid to transform <see cref="List<KeyValuePair<string, object>>"/> instance to an array of <see cref="IDataParameter"/>
        /// </summary>
        /// <param name="parameters">instance the extension is applyed to</param>
        /// <returns></returns>
        public static IDataParameter[] ToDataParameters(this List<KeyValuePair<string, object>> parameters)
        {
            return ToDataParameters(parameters.ToArray());
        }

        /// <summary>
        /// Get <see cref="SqlParameter[]"/> based on provided <see cref="PropertyInfo[]"/>
        /// </summary>
        /// <param name="properties">Array of <see cref="PropertyInfo"/> to generate array of <see cref="SqlParameter"/>.</param>
        /// <param name="parameters">Ann ojbect containinng parameters</param>
        /// <returns></returns>
        private static IDataParameter[] PropertyInfoToSqlParameters(PropertyInfo[] properties, object parameters)
        {
            SqlParameter[] sqlParameters = null;

            sqlParameters = new SqlParameter[properties.Length];

            PropertyInfo propInfo;

            for (int i = 0, len = properties.Length; i < len; i++)
            {
                propInfo = properties[i];
                object val = propInfo.GetValue(parameters);
                sqlParameters[i] = new SqlParameter(propInfo.Name, val == null ? DBNull.Value : val);
            }

            return sqlParameters;
        }

        /// <summary>
        /// Extension methid to transform <see cref="dynamic"/> instance to an array of <see cref="IDataParameter"/>
        /// </summary>
        /// <param name="parameters">instance the extension is applyed to</param>
        /// <returns></returns>
        public static IDataParameter[] ToDataParameters(dynamic parameters)
        {
            if (parameters == null)
                return null;

            PropertyInfo[] properties = parameters.GetType().GetProperties();

            return PropertyInfoToSqlParameters(properties, parameters);
        }

        /// <summary>
        /// Extension methid to transform generic type instance to an array of <see cref="IDataParameter"/>
        /// </summary>
        /// <typeparam name="T">Generic  type of the instance</typeparam>
        /// <param name="parameters">item containing properties interpreted as parameters</param>
        /// <returns></returns>
        public static IDataParameter[] ToDataParameters<T>(this T parameters)
        {
            if (parameters == null)
                return null;

            PropertyInfo[] properties = PropertyInfoCache.GetProperties(typeof(T));

            return PropertyInfoToSqlParameters(properties, parameters);
        }
    }
}

