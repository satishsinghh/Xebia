using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xebia.DatabaseCore.Common
{
    public class XebiaDatabase
        : SqlDatabase, IXebiaDatabase
    {
        public XebiaDatabase(IXebiaDatabaseConnection connection)
            : base(connection)
        {

        }

        public virtual PagedList<TResult> GetPagedCollection<TResult>(
            string procedure,
            IEnumerable<QueryParameter> parameters,
            Action<IDataReader, List<TResult>> handleRemainingResultSets = null)
            where TResult : new()
        {
            var results = new List<TResult>();
            var queryParameters = parameters.ToDictionary(x => x.Name);

            //ArgumentContracts.Assert(queryParameters.ContainsKey(Constants.StartRecordParameter), "parameters", "parameters must include " + Constants.StartRecordParameter + ".");
            //ArgumentContracts.Assert(queryParameters.ContainsKey(Constants.RecordsPerPageParameter), "parameters", "parameters must include " + Constants.RecordsPerPageParameter + ".");
            //ArgumentContracts.Assert(queryParameters.ContainsKey(Constants.TotalRecordsParameter), "parameters", "parameters must include " + Constants.TotalRecordsParameter + ".");

            using (var reader = ExecuteDataReader(procedure, queryParameters.Values.ToArray()))
            {
                while (reader.Read())
                {
                    var element = reader.GetObject<TResult>();
                    results.Add(element);
                }

                if (handleRemainingResultSets != null)
                {
                    handleRemainingResultSets(reader, results);
                }
            }

            var skip = queryParameters[Constants.StartRecordParameter].GetValue<int>();
            var take = queryParameters[Constants.RecordsPerPageParameter].GetValue<int>();
            var totalCount = queryParameters[Constants.TotalRecordsParameter].GetValue<int>();

            return new PagedList<TResult>(results,
                                          totalCount,
                                          skip,
                                          take);
        }
    }
}