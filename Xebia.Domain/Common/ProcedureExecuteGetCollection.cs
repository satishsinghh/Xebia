using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xebia.DatabaseCore.Common
{
    public abstract class ProcedureExecuteGetCollection<TResult>
        : Procedure<IList<TResult>>
        where TResult : new()
    {
        protected readonly IXebiaDatabase database;

        public ProcedureExecuteGetCollection(IXebiaDatabase database)
        {
            this.database = database;
        }

        public override IList<TResult> Execute()
        {
            var results = default(IList<TResult>);
            var name = this.GetName();
            var parameters = this.GetQueryParameters();

            using (var reader = database.ExecuteDataReader(name, parameters.ToArray()))
            {
                results = reader.GetCollection<TResult>().ToList();
            }

            this.UpdateOutputValues();

            return results;
        }
    }
}
