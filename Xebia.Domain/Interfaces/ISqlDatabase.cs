using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Xebia.DatabaseCore.Common;

namespace Xebia.DatabaseCore
{
    public interface ISqlDatabase
        : IDisposable
    {
        IDataReader ExecuteDataReader(string procedure, params QueryParameter[] parameters);

        /// <summary>
        /// Executes procedure with given query parameters
        /// Note : CommandTimeout depends on implementation
        /// </summary>
        /// <param name="procedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteQuery(string procedure, params QueryParameter[] parameters);

        Task<int> ExecuteQueryAsync(string procedure, int? timeout, params QueryParameter[] parameters);

        T ExecuteScalar<T>(string procedure, params QueryParameter[] parameters);

        void ThrowEntityNotFoundExceptions(bool throwExceptions);
        bool ThrowsEntityNotFoundExceptions();
    }
}
