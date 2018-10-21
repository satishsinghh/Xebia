using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xebia.DatabaseCore.Common
{
    public abstract class AuditProcedureExecuteReader<TResult>
        : Procedure<TResult>
    {
        protected readonly IXebiaAuditDatabase database;
		protected readonly IXebiaDatabase xebiaDatabase;

		public AuditProcedureExecuteReader(IXebiaAuditDatabase _database, IXebiaDatabase _xebiaDatabase)
        {
            this.database = _database;
			this.xebiaDatabase = _xebiaDatabase;

		}

        public override TResult Execute()
        {
            TResult results = default(TResult);
            var name = this.GetName();
            var parameters = this.GetQueryParameters();

            using (var reader = database.ExecuteDataReader(name, parameters.ToArray()))
            {
                results = HandleDataReader(reader, parameters);
            }

            this.UpdateOutputValues();
            return results;
        }

        public abstract TResult HandleDataReader(IDataReader reader, IEnumerable<QueryParameter> parameters);
    }
}