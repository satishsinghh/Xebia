using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Xebia.DatabaseCore.Common
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Extracts the value from a ParameterCollection by index and converts to T
        /// </summary>
        public static T GetValue<T>(this IDataParameterCollection collection, int index)
        {
            var parameter = (IDataParameter)collection[index];
            return parameter.Value.ConvertTo<T>();
        }

        /// <summary>
        /// Extracts the value from a ParameterCollection by name and converts to T
        /// </summary>
        public static T GetValue<T>(this IDataParameterCollection collection, string name)
        {
            var parameter = (IDataParameter)collection[name];
            return parameter.Value.ConvertTo<T>();
        }

        /// <summary>
        /// Extracts the value from the record in a DataReader by column name and converts to T
        /// </summary>
        public static T GetValue<T>(this IDataReader reader, string column)
        {
            var type = typeof(T);

            var result = GetValue(reader, column, type);

            if (result == null)
            {
                return default(T);
            }

            return (T)result;
        }

        /// <summary>
        /// Extracts the value from the current the record in a DataReader by column name and converts to returnType
        /// </summary>
        public static object GetValue(this IDataReader reader, string column, Type returnType)
        {
            var data = reader[column];

            if (data == null || data == DBNull.Value)
            {
                return null;
            }

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                returnType = Nullable.GetUnderlyingType(returnType);
            }

            if (returnType.IsEnum)
            {
                if (data is string)
                {
                    return Enum.Parse(returnType, data.ToString(), true);
                }
                else
                {
                    return Enum.ToObject(returnType, data);
                }
            }
            else if (data.GetType() != returnType)
            {
                return Convert.ChangeType(data, returnType);
            }

            return data;
        }

        /// <summary>
        /// Extracts a full object from the current record in a DataReader, mapping return columns to object properties.
        /// Allows custom mappings of DataReader field values to an object result value via customMappings argument.
        /// </summary>
        public static T GetObject<T>(this IDataReader reader, IDictionary<string, Func<IDataReader, string, object>> customMappings = null, DataMappingOptions options = null)
            where T : new()
        {
            Type type = typeof(T);
            T obj = new T();

            if (options == null) options = new DataMappingOptions();

            var schema = reader.GetSchemaTable();

            foreach (DataRow row in schema.Rows)
            {
                var column = row["ColumnName"].ToString();
                var ordinal = Convert.ToInt32(row["ColumnOrdinal"]);
                var dbType = (Type)row["DataType"];
                var prop = type.GetProperty(column);

                if (prop != null && prop.CanWrite)
                {
                    if (customMappings != null && customMappings.ContainsKey(column))
                    {
                        object value = customMappings[column](reader, column);
                        prop.SetValue(obj, value);
                    }
                    else
                    {
                        object value = GetValueFromDataReader(reader, ordinal, dbType, prop.PropertyType, options);

                        if (value != null)
                        {
                            prop.SetValue(obj, value);
                        }
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// Extracts a full object from the current record in a DataReader, mapping return columns to object constructer arguments.
        /// Use this for mapping to immutable types.
        /// </summary>
        public static T ConstructObject<T>(this IDataReader reader, DataMappingOptions options = null)
        {
            if (options == null) options = new DataMappingOptions();

            Type type = typeof(T);

            var ctor = type.GetConstructors()
                           .OrderByDescending(x => x.GetParameters().Length)
                           .FirstOrDefault();

            var ctorParameters = ctor.GetParameters();

            var columns = from DataRow row in reader.GetSchemaTable().Rows
                          select new
                          {
                              Name = row["ColumnName"].ToString().ToLower(),
                              Ordinal = Convert.ToInt32(row["ColumnOrdinal"]),
                              DatabaseType = (Type)row["DataType"]
                          };

            var mappings = (from parameter in ctorParameters
                            join column in columns on parameter.Name.ToLower() equals column.Name
                            select new
                            {
                                Ordinal = column.Ordinal,
                                DatabaseType = column.DatabaseType,
                                ParameterType = parameter.ParameterType
                            }).ToList();

            // Validate
            if (ctorParameters.Length > mappings.Count)
            {
                throw new InvalidOperationException(
                    "Invalid mappings detected.  Attempting to construct an object of type " + type.FullName + " with missing parameter information.\r\n" +
                    "Ctor parameters: " + String.Join(", ", ctorParameters.Select(x => string.Format("{0} {1}", x.ParameterType, x.Name))) + "\r\n" +
                    "Database fields: " + String.Join(", ", columns.Select(x => string.Format("{0} {1}", x.DatabaseType, x.Name))));
            }

            var parameterValues = mappings.Select(mapping => GetValueFromDataReader(reader, mapping.Ordinal, mapping.DatabaseType, mapping.ParameterType, options))
                                          .ToArray();

            var result = ctor.Invoke(parameterValues);

            return (T)result;
        }

        /// <summary>
        /// Calls DataReader.Read() on the input reader, and then extracts a full object from that record by property mapping (see GetObject`T)
        /// </summary>
        public static T GetNextObject<T>(this IDataReader reader, IDictionary<string, Func<IDataReader, string, object>> customMappings = null)
            where T : new()
        {
            if (reader.Read())
            {
                return reader.GetObject<T>(customMappings);
            }

            return default(T);
        }

        /// <summary>
        /// Calls DataReader.NextResult() and DataReader.Read() on the input reader, and then extracts a full object from that record by property mapping (see GetObject`T)
        /// </summary>
        public static T GetNextResultSetObject<T>(this IDataReader reader, IDictionary<string, Func<IDataReader, string, object>> customMappings = null)
            where T : new()
        {
            if (reader.NextResult())
            {
                if (reader.Read())
                {
                    return reader.GetObject<T>(customMappings);
                }
            }

            return default(T);
        }


        /// <summary>
        /// Calls DataReader.Read() on the input reader, and then extracts a full object from that record by constructor (see ConstructObject`T)
        /// </summary>
        public static T ConstructNextObject<T>(this IDataReader reader)
        {
            if (reader.Read())
            {
                return reader.ConstructObject<T>();
            }

            return default(T);
        }


        /// <summary>
        /// Calls DataReader.NextResult() and DataReader.Read() on the input reader, and then extracts a full object from that record by constructor (see ConstructObject`T)
        /// </summary>
        public static T ConstructNextResultSetObject<T>(this IDataReader reader)
        {
            if (reader.NextResult())
            {
                if (reader.Read())
                {
                    return reader.ConstructObject<T>();
                }
            }

            return default(T);
        }

        /// <summary>
        /// Calls DataReader.Read() in a loop and yields an object per record using property mapping (see GetObject`T)
        /// </summary>
        public static IEnumerable<T> GetCollection<T>(this IDataReader reader, IDictionary<string, Func<IDataReader, string, object>> customMappings = null)
            where T : new()
        {
            while (reader.Read())
            {
                yield return reader.GetObject<T>(customMappings);
            }
        }


        /// <summary>
        /// Calls DataReader.Read() in a loop and yields an object per record by constructor (see ConstructObject`T)
        /// </summary>
        public static IEnumerable<T> ConstructCollection<T>(this IDataReader reader)
        {
            while (reader.Read())
            {
                yield return reader.ConstructObject<T>();
            }
        }

        /// <summary>
        /// Calls DataReader.NextResult() to move to the next recordset, then calls DataReader.Read() in a loop and yields an object per record using property mapping (see GetObject`T)
        /// </summary>
        public static IEnumerable<T> GetNextResultSetCollection<T>(this IDataReader reader, IDictionary<string, Func<IDataReader, string, object>> customMappings = null)
            where T : new()
        {
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    yield return reader.GetObject<T>(customMappings);
                }
            }
        }


        /// <summary>
        /// Calls DataReader.NextResult() to move to the next recordset, then calls DataReader.Read() in a loop and yields an object per record by constructor (see ConstructObject`T)
        /// </summary>
        public static IEnumerable<T> ConstructNextResultSetCollection<T>(this IDataReader reader)
        {
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    yield return reader.ConstructObject<T>();
                }
            }
        }

        //public static DbConnection CreateConnection(this ITmpConfigurationManager config, string connectionStringKey)
        //{
        //   // ArgumentContracts.AssertNotNull(config, "config");
        // //   ArgumentContracts.AssertNotNullOrWhiteSpace(connectionStringKey, "connectionStringKey");

        //    var provider = config.GetConnectionStringProvider(connectionStringKey);

        //    if (provider == null)
        //    {
        //        throw new ArgumentException("Invalid connection string name " + connectionStringKey + ".  Missing providerName.", "connectionString");
        //    }

        //    var factory = DbProviderFactories.GetFactory(provider);

        //    var connection = factory.CreateConnection();

        //    connection.ConnectionString = config.GetConnectionString(connectionStringKey);

        //    return connection;
        //}

        public static DbCommand CreateCommand(this DbConnection connection, string sql, CommandType commandType)
        {
            var command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = commandType;

            return command;
        }

        //public static DbCommand CreateCommand(this DbContext context, string sql, CommandType commandType)
        //{
        //    var command = context.Database.Connection.CreateCommand();
        //    command.CommandText = sql;
        //    command.CommandType = commandType;

        //    return command;
        //}

        public static DbCommand CreateStoredProcedureCommand(this DbConnection connection, string sql)
        {
            return CreateCommand(connection, sql, CommandType.StoredProcedure);
        }

        //public static DbCommand CreateStoredProcedureCommand(this DbContext context, string sql)
        //{
        //    return CreateCommand(context, sql, CommandType.StoredProcedure);
        //}

        public static DbCommand AppendInputParameter(this DbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);

            return command;
        }

        public static DbCommand AppendInputParameter(this DbCommand command, string name, object value, DbType type)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            parameter.DbType = type;
            command.Parameters.Add(parameter);

            return command;
        }

        public static DbCommand AppendInputParameter(this DbCommand command, string name, DataTable value, string typeName)
        {
            var parameter = value.ToSqlParameter(name, typeName);
            command.Parameters.Add(parameter);

            return command;
        }

        public static DbCommand AppendOutputParameter(this DbCommand command, string name, DbType type)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = type;
            parameter.Direction = ParameterDirection.Output;
            command.Parameters.Add(parameter);

            return command;
        }

        public static DbCommand AppendParameter(this DbCommand command, string name, DbType type, object value, ParameterDirection direction)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.DbType = type;
            parameter.Value = value ?? DBNull.Value;
            parameter.Direction = direction;
            command.Parameters.Add(parameter);

            return command;
        }

        public static DbCommand SetCommandTimeout(this DbCommand command, TimeSpan timeSpan)
        {
            command.CommandTimeout = (int)timeSpan.TotalSeconds;

            return command;
        }

        public static IDataReader OpenConnectionAndExecute(this DbCommand command)
        {
            ConnectionStateHandler.OpenConnectionIfClosed(command.Connection);

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static IDataReader OpenConnectionAndExecute(this DbCommand command, CommandBehavior commandBehavior)
        {
            ConnectionStateHandler.OpenConnectionIfClosed(command.Connection);
            return command.ExecuteReader(commandBehavior);
        }

        public static int OpenConnectionAndExecuteNonQuery(this DbCommand command)
        {
            using (command.Connection.OpenConnection())
            {
                return command.ExecuteNonQuery();
            }
        }

        public static TResult OpenConnectionAndExecuteScalar<TResult>(this DbCommand command)
        {
            object results = null;

            using (command.Connection.OpenConnection())
            {
                results = command.ExecuteScalar();
            }

            if (ReferenceEquals(results, null) || results == DBNull.Value)
            {
                return default(TResult);
            }

            return results.ConvertTo<TResult>();
        }

        public static SqlParameter ToSqlParameter(this DataTable table, string name, string typeName)
        {
            var parameter = new SqlParameter(name, SqlDbType.Structured);
            parameter.Value = table;
            parameter.TypeName = typeName;

            return parameter;
        }

        public static SqlParameter ToSqlParameter(this object obj, string name, SqlDbType type, ParameterDirection direction = ParameterDirection.Input)
        {
            var parameter = new SqlParameter(name, type);
            parameter.Value = ReferenceEquals(obj, null) ? DBNull.Value : obj;
            parameter.Direction = direction;

            return parameter;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="parameters">The parameters.</param>
        public static void AddRange(this IDataParameterCollection collection, IEnumerable<IDataParameter> parameters)
        {
            if (parameters == null)
            {
                return;
            }
            foreach (var parameter in parameters)
            {
                collection.Add(parameter);
            }
        }

        public static ConnectionStateHandler OpenConnection(this IDbConnection connection)
        {
            return new ConnectionStateHandler(connection);
        }

        private static object GetValueFromDataReader(IDataReader reader, int column, Type sourceType, Type destinationType, DataMappingOptions options)
        {
            object value = reader.GetValue(column);

            if (value == DBNull.Value)
            {
                value = null;
            }

            if (value != null)
            {
                if (sourceType != destinationType)
                {
                    if (destinationType.IsGenericType && destinationType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        destinationType = Nullable.GetUnderlyingType(destinationType);
                    }
                }

                if (sourceType == typeof(DateTime))
                {
                    //all of our dataTime values from SQL Server will be interpreted as local time because there is no offset stored, but many of the values are actually UTC, so we need to be able to read them correctly
                    value = DateTime.SpecifyKind((DateTime)value, options.ReadDateTimeAs);
                }

                if (destinationType.IsEnum)
                {
                    if (value is string)
                    {
                        value = Enum.Parse(destinationType, value.ToString(), true);
                    }
                    else
                    {
                        var iValue = Convert.ChangeType(value, Enum.GetUnderlyingType(destinationType));
                        value = Enum.ToObject(destinationType, iValue);
                    }
                }
                else if (sourceType != destinationType)
                {
                    if (destinationType == typeof(string))
                    {
                        value = value.ToString();
                    }
                    else
                    {
                        value = Convert.ChangeType(value, destinationType);
                    }
                }
            }

            return value;
        }
    }
}
