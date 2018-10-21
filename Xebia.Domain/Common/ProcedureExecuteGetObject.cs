using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xebia.DatabaseCore.Common
{
    public abstract class ProcedureExecuteGetObject<TResult>
        : Procedure<TResult>
        where TResult : new()
    {
        protected readonly IXebiaDatabase database;

        public ProcedureExecuteGetObject(IXebiaDatabase database)
        {
            this.database = database;
        }

        public override TResult Execute()
        {
            TResult results = default(TResult);
            var name = this.GetName();
            var parameters = this.GetQueryParameters();

            using (var reader = database.ExecuteDataReader(name, parameters.ToArray()))
            {
                results = reader.GetObject<TResult>();
            }

            this.UpdateOutputValues();

            return results;
        }
    }
}
