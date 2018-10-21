using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xebia.DatabaseCore.Common
{
    public abstract class Procedure<TResult>
        : IProcedure<TResult>
    {
        private List<QueryParameter> parameters = new List<QueryParameter>();
        private List<Action> returnValueCallbacks = new List<Action>();

        public Procedure()
        {
        }

        public abstract string GetName();
        public abstract TResult Execute();

        public IEnumerable<QueryParameter> GetQueryParameters()
        {
            Build();

            return parameters;
        }

        public void UpdateOutputValues()
        {
            foreach (var callback in returnValueCallbacks)
            {
                callback();
            }
        }

        private void Build()
        {
            parameters.Clear();
            returnValueCallbacks.Clear();

            foreach (var property in this.GetType().GetProperties())
            {
                if (property.CanRead)
                {
                    string parameterName = "@" + property.Name;
                    var parameterDirectionAttrib = property.GetCustomAttributes(typeof(ParameterDirectionAttribute), false).FirstOrDefault() as ParameterDirectionAttribute;

                    if (parameterDirectionAttrib == null || parameterDirectionAttrib.ParameterDirection == ParameterDirection.Input)
                    {
                        if (typeof(DataTable).IsAssignableFrom(property.PropertyType))
                        {
                            var value = property.GetValue(this) as DataTable;

                            if (value != null)
                            {
                                this.parameters.Add(new QueryParameter(parameterName, value.TableName, value));
                            }
                        }
                        else
                        {
                            DbType dbType;
                            Type underlyingType = DbTypeMap.GetUnderlyingType(property.PropertyType);

                            if (DbTypeMap.TryGetValue(underlyingType, out dbType))
                            {
                                object value = GetValue(property, underlyingType);

                                this.parameters.Add(new QueryParameter(parameterName, dbType, value));
                            }
                        }
                    }
                    else
                    {
                        if (!property.CanWrite)
                        {
                            throw new InvalidOperationException("Error: cannot declare a property as Output, InputOutput return ReturnValue if it is not writable.");
                        }

                        DbType dbType;
                        Type underlyingType = DbTypeMap.GetUnderlyingType(property.PropertyType);

                        if (DbTypeMap.TryGetValue(underlyingType, out dbType))
                        {
                            object value = (parameterDirectionAttrib.ParameterDirection == ParameterDirection.InputOutput)
                                                    ? GetValue(property, underlyingType)
                                                    : DBNull.Value;

                            var parameter = new QueryParameter(parameterName, dbType, value, parameterDirectionAttrib.ParameterDirection);

                            this.parameters.Add(parameter);

                            this.returnValueCallbacks.Add(() =>
                            {
                                SetValue(property, parameter.Value);
                            });
                        }
                    }
                }
            }
        }

        private object GetValue(PropertyInfo property, Type underlyingType)
        {
            object value = property.GetValue(this) ?? DBNull.Value;

            if (value != DBNull.Value && underlyingType != property.PropertyType)
            {
                value = Convert.ChangeType(value, underlyingType);
            }

            return value;
        }

        private void SetValue(PropertyInfo property, object value)
        {
            if (value == null || value == DBNull.Value)
            {
                value = null;
            }
            else if (value.GetType() != property.PropertyType)
            {
                Type underlyingType = DbTypeMap.GetUnderlyingType(property.PropertyType);

                value = Convert.ChangeType(value, underlyingType);
            }

            property.SetValue(this, value);
        }
    }
}
