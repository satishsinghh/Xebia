using Microsoft.Extensions.Options;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using Xebia.Model;

namespace Xebia.DatabaseCore.Common
{
    public class XebiaAuditConnection : IXebiaAuditConnection
    {
        private readonly SqlConnection connection;

        public XebiaAuditConnection(IOptions<ConnectionStrings> options)
        {
            this.connection = new SqlConnection(options.Value.JobDistributionAudit);
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
