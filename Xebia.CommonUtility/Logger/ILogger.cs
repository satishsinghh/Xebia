using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.CommonUtility.Logger
{
    public interface ILogger
    {
        void Log(LogEntry entry);
        void Log(LogEntry entry, HttpContext httpContext);
        void Log(HttpContext httpContext, string body = "");
    }
}
