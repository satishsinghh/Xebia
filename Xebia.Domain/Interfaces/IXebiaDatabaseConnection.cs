using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Xebia.DatabaseCore
{
    public interface IXebiaDatabaseConnection
        : IDisposable
    {
        DbConnection GetConnection();
    }
}
