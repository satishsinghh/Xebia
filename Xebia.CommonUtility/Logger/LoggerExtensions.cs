using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.CommonUtility.Logger
{
    public static class LoggerExtensions
    {
        public static void Log(this ILogger logger, string message)
        {
            logger.Log(new LogEntry(LoggingEventType.Information, message));
        }
        public static void Log(this ILogger logger, Exception exception)
        {
            logger.Log(new LogEntry(LoggingEventType.Error, exception.Message, exception));
        }
        public static void Log(this ILogger logger, Exception exception, HttpContext httpContext)
        {
            logger.Log(new LogEntry(LoggingEventType.Error, exception.Message, exception), httpContext);
        }

    }
}
