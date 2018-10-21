using Microsoft.Extensions.Options;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using Xebia.Model;

namespace Xebia.DatabaseCore.Common
{
    public class XebiaDatabaseConnection
        : IXebiaDatabaseConnection
    {
        private readonly SqlConnection connection;

        public XebiaDatabaseConnection(IOptions<ConnectionStrings> options)
        {
            this.connection = new SqlConnection(options.Value.JobDistribution);
        }

        public DbConnection GetConnection()
        {
            return this.connection;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                connection.Dispose();
            }
        }
    }
}