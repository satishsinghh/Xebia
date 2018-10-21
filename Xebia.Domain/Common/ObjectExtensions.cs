using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.DatabaseCore.Common
{
    public static class ObjectExtensions 
    {
        /// <summary>
        /// Converts an input object to TOutput, with support for automatic conversion from null/DBNull to default(T), as well as handling of nullable types and enums.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Output object.</returns>
        public static TOutput ConvertTo<TOutput>(this object value)
        {
            Type type = typeof(TOutput);

            if (value == null || value == DBNull.Value)
            {
                return default(TOutput);
            }
            else if (value is TOutput)
            {
                return (TOutput)value;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            if (type.IsEnum)
            {
                if (value is string)
                {
                    return (TOutput)Enum.Parse(type, value.ToString());
                }
                else
                {
                    var underlyingType = Enum.GetUnderlyingType(type);
                    value = Convert.ChangeType(value, underlyingType);
                    return (TOutput)Enum.ToObject(type, value);
                }
            }

            return (TOutput)Convert.ChangeType(value, type);
        }

        /// <summary>
        /// Converts an input object to TOutput, with support for automatic conversion from null/DBNull to default(T), as well as handling of nullable types and enums.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Output object.</returns>
        public static object ConvertTo(this object value, Type type)
        {
            if (value == null || value == DBNull.Value)
            {
                if (type.IsValueType)
                    return Activator.CreateInstance(type);
                else
                    return null;
            }
            else if (value.GetType() == type)
            {
                return value;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            if (type.IsEnum)
            {
                if (value is string)
                {
                    return Enum.Parse(type, value.ToString());
                }
                else
                {
                    var underlyingType = Enum.GetUnderlyingType(type);
                    value = Convert.ChangeType(value, underlyingType);
                    return Enum.ToObject(type, value);
                }
            }

            return Convert.ChangeType(value, type);
        }

        /// <summary>
        /// Calls the object's ToString method.  If the object is null, then returns an empty string.
        /// </summary>
        public static string ToStringSafe(this object obj)
        {
            return (obj != null) ? obj.ToString() : string.Empty;
        }

        /// <summary>
        /// Throws an ArgumentNullException if the current object is null.
        /// </summary>
        public static void ThrowIfNull(this object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Returns an object's hash code, or zero if the object is null
        /// </summary>
        public static int GetHashCodeSafe<T>(this T obj)
            where T : class
        {
            if (obj == null)
            {
                return 0;
            }

            return obj.GetHashCode();
        }
    }
}
