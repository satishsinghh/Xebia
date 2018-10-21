using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xebia.DatabaseCore.Common
{
    public abstract class ProcedureExecuteScalar<TResult>
        : Procedure<TResult>
    {
        protected readonly IXebiaDatabase database;

        public ProcedureExecuteScalar(IXebiaDatabase database)
        {
            this.database = database;
        }

        public override TResult Execute()
        {
            var name = this.GetName();
            var parameters = this.GetQueryParameters();

            TResult result = database.ExecuteScalar<TResult>(name, parameters.ToArray());

            this.UpdateOutputValues();

            return result;
        }
    }
}
