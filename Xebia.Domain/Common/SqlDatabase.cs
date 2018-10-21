using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Xebia.DatabaseCore.Common
{
    public class SqlDatabase
        : ISqlDatabase 
    {
        protected int? commandTimeout;
        private bool throwNotFoundExceptions = true;

        private readonly IXebiaDatabaseConnection connection;

        public SqlDatabase(IXebiaDatabaseConnection connection)
        {
            this.connection = connection;
        }

        public void ThrowEntityNotFoundExceptions(bool throwExceptions)
        {
            this.throwNotFoundExceptions = throwExceptions;
        }

        public bool ThrowsEntityNotFoundExceptions()
        {
            return throwNotFoundExceptions;
        }

        public IDataReader ExecuteDataReader(string procedure, params QueryParameter[] parameters)
        {
            var connection = this.GetConnection();

            var command = this.GetConnection().CreateStoredProcedureCommand(procedure);

            command.CommandTimeout = commandTimeout ?? command.CommandTimeout;

            SetIncomingParameterValues(command, parameters);

            StateChangeEventHandler stateChangedHandler = null;

            stateChangedHandler = (_s, _e) =>
            {
                if (_e.CurrentState == ConnectionState.Closed)
                {
                    SetOutputParameterValues(command, parameters);

                    connection.StateChange -= stateChangedHandler;
                }
            };

            connection.StateChange += stateChangedHandler;

            return command.OpenConnectionAndExecute(CommandBehavior.CloseConnection);
        }

        public TResult ExecuteSingleResult<TResult>(string procedure, params QueryParameter[] parameters)
            where TResult : new()
        {
            using (var reader = ExecuteDataReader(procedure, parameters))
            {
                if (reader.Read())
                {
                    return reader.GetObject<TResult>();
                }
            }

            if (ThrowsEntityNotFoundExceptions())
            {
                throw DatabaseEntityNotFoundException.Create(typeof(TResult), parameters);
            }
            else
            {
                return default(TResult);
            }
        }

        public TResult ConstructResult<TResult>(string procedure, params QueryParameter[] parameters)
        {
            using (var reader = ExecuteDataReader(procedure, parameters))
            {
                if (reader.Read())
                {
                    return reader.ConstructObject<TResult>();
                }
            }

            if (ThrowsEntityNotFoundExceptions())
            {
                throw DatabaseEntityNotFoundException.Create(typeof(TResult), parameters);
            }
            else
            {
                return default(TResult);
            }
        }

        public IEnumerable<TResult> ExecuteCollection<TResult>(string procedure, params QueryParameter[] parameters)
            where TResult : new()
        {
            using (var reader = ExecuteDataReader(procedure, parameters))
            {
                while (reader.Read())
                {
                    yield return reader.GetObject<TResult>();
                }
            }
        }

        public IEnumerable<TResult> ConstructCollection<TResult>(string procedure, params QueryParameter[] parameters)
        {
            using (var reader = ExecuteDataReader(procedure, parameters))
            {
                while (reader.Read())
                {
                    yield return reader.ConstructObject<TResult>();
                }
            }
        }

        /// <summary>
        /// Executes procedure with given query parameters. 
        /// Uses commandTimeout on instance if not null.
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteQuery(string procedure, params QueryParameter[] parameters)
        {
            return ExecuteQuery(procedure, commandTimeout, parameters);
        }

        /// <summary>
        /// Executes procedure with given query parameters. 
        /// Uses given timeout value as the CommandTimeout if not null, otherwise uses default CommandTimeout (30 seconds according to msdn) on DbCommand.
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="timeout">Command timeout in seconds</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteQuery(string procedure, int? timeout, params QueryParameter[] parameters)
        {
            var command = this.GetConnection().CreateStoredProcedureCommand(procedure);

            command.CommandTimeout = timeout ?? command.CommandTimeout;

            SetIncomingParameterValues(command, parameters);

            var result = command.OpenConnectionAndExecuteNonQuery();

            SetOutputParameterValues(command, parameters);

            return result;
        }

        public async Task<int> ExecuteQueryAsync(string procedure, int? timeout, params QueryParameter[] parameters)
        {
            var command = this.GetConnection().CreateStoredProcedureCommand(procedure);

            command.CommandTimeout = timeout ?? command.CommandTimeout;

            SetIncomingParameterValues(command, parameters);

            if (command.Connection.State == ConnectionState.Closed)
                await command.Connection.OpenAsync();

            var result = command.ExecuteNonQueryAsync();

            SetOutputParameterValues(command, parameters);

            if (command.Connection.State == ConnectionState.Open)
                command.Connection.Close();

            return await result;
        }

        public T ExecuteScalar<T>(string procedure, params QueryParameter[] parameters)
        {
            var command = this.GetConnection().CreateStoredProcedureCommand(procedure);

            command.CommandTimeout = commandTimeout ?? command.CommandTimeout;

            SetIncomingParameterValues(command, parameters);

            var result = command.OpenConnectionAndExecuteScalar<T>();

            SetOutputParameterValues(command, parameters);

            return result;
        }

        /// <summary>
        /// Sets command input parameters
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        public void SetIncomingParameterValues(DbCommand command, IEnumerable<QueryParameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                if (!String.IsNullOrWhiteSpace(parameter.StructedDataTableType) && parameter.Value is DataTable)
                {
                    command.AppendInputParameter(parameter.Name, (DataTable)parameter.Value, parameter.StructedDataTableType);
                }
                else
                {
                    command.AppendParameter(parameter.Name, parameter.DbType, parameter.Value, parameter.ParameterDirection);
                }
            }
        }

        /// <summary>
        /// Sets command output parameters
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        public void SetOutputParameterValues(DbCommand command, IEnumerable<QueryParameter> parameters)
        {
            foreach (var parameter in parameters.Where(x => x.ParameterDirection != ParameterDirection.Input))
            {
                var dbParameter = command.Parameters.Cast<DbParameter>().FirstOrDefault(x => x.ParameterName == parameter.Name);

                if (dbParameter != null)
                {
                    parameter.Value = dbParameter.Value;
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected DbConnection GetConnection()
        {
            var dbConnection = connection.GetConnection();

            return dbConnection;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.connection.Dispose();
            }
        }
    }
}