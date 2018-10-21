using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xebia.DatabaseCore.Common
{
    public abstract class ProcedureExecuteQuery
        : Procedure<int>
    {
        protected readonly ISqlDatabase database;

        public ProcedureExecuteQuery(ISqlDatabase database)
        {
            this.database = database;
        }

        public override int Execute()
        {
            var name = this.GetName();
            var parameters = this.GetQueryParameters();

            int count = database.ExecuteQuery(name, parameters.ToArray());

            this.UpdateOutputValues();

            return count;
        }

        public async Task<int> ExecuteAsync() 
        {
            var name = this.GetName();
            var parameters = this.GetQueryParameters();

            int count = await database.ExecuteQueryAsync(name, null, parameters.ToArray());

            this.UpdateOutputValues();

            return count;
        }
    }
}
