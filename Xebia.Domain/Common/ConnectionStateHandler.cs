using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Xebia.DatabaseCore.Common
{
    public class ConnectionStateHandler
        : IDisposable
    {
        private bool connectionShouldBeClosed;
        private readonly IDbConnection connection;

        public ConnectionStateHandler(IDbConnection connection)
        {
            this.connection = connection;
            this.connectionShouldBeClosed = OpenConnectionIfClosed(connection);
        }

        public static bool OpenConnectionIfClosed(IDbConnection connection)
        {
            if (connection != null && connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.connection.State == System.Data.ConnectionState.Open && connectionShouldBeClosed)
                {
                    this.connection.Close();
                }
            }
        }
    }
}
