using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xebia.DatabaseCore.Common;

namespace Xebia.DatabaseCore
{
    public interface IProcedure
    {
        string GetName();
        IEnumerable<QueryParameter> GetQueryParameters();
        void UpdateOutputValues();
    }

    public interface IProcedure<TResult>
        : IProcedure
    {
        TResult Execute();
    }
}
