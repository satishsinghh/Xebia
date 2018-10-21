using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xebia.DatabaseCore.Common
{
    public class QueryParameter 
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public DbType DbType { get; set; }
        public string StructedDataTableType { get; set; }
        public ParameterDirection ParameterDirection { get; set; }

        public QueryParameter(string name, DbType type, object value)
        {
            this.Name = name;
            this.DbType = type;
            this.Value = value;
            this.ParameterDirection = System.Data.ParameterDirection.Input;
        }

        public QueryParameter(string name, DbType type, object value, ParameterDirection direction)
        {
            this.Name = name;
            this.DbType = type;
            this.Value = value;
            this.ParameterDirection = direction;
        }

        public QueryParameter(string name, string tableName, DataTable data)
        {
            this.Name = name;
            this.StructedDataTableType = tableName;
            this.Value = data;
            this.ParameterDirection = System.Data.ParameterDirection.Input;
        }

        public T GetValue<T>()
        {
            return Value.ConvertTo<T>();
        }
    }
}
