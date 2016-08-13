using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Swiss
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Method returns whether this type is nullable
        /// </summary>
        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// Method converts this CLR type into a SQL type
        /// </summary>
        public static SqlDbType ToSqlType(this Type type)
        {
            string clrType = type.Name.ToLower();
            SqlDbType sqlType = SqlDbType.NVarChar;

            switch (clrType)
            {
                case "int32":
                    sqlType = SqlDbType.Int;
                    break;
                case "double":
                    sqlType = SqlDbType.Float;
                    break;
                case "int16":
                    sqlType = SqlDbType.SmallInt;
                    break;
                case "int64":
                    sqlType = SqlDbType.BigInt;
                    break;
                case "float":
                    sqlType = SqlDbType.Float;
                    break;
                case "datetime":
                    sqlType = SqlDbType.DateTime;
                    break;
            }

            return sqlType;
        }

        /// <summary>
        /// Method gathers property names and types of this type
        /// </summary>
        public static Dictionary<string, Type> GetPropertyNamesAndTypes(this Type incomingType)
        {
            Dictionary<string, Type> result = new Dictionary<string, Type>();
            IList<PropertyInfo> properties = new List<PropertyInfo>(incomingType.GetProperties());

            foreach (PropertyInfo prop in properties)
            {
                string propName = prop.Name;
                Type propType = prop.PropertyType;

                result.Add(propName, propType);
            }

            return result;
        }

        /// <summary>
        /// Method returns whether this type is an enumerable of some sort
        /// </summary>
        public static bool IsEnumerable(this Type type)
        {
            return type.GetInterface("IEnumerable") != null && !type.Equals(typeof(String));
        }
    }
}
