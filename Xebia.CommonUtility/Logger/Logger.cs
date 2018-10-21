using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Internal;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.CommonUtility.Logger
{
    public class Logger : ILogger
    {
        private const string INFO = "Info";
        private const string EXCEPTION = "Exception";

        private NLog.ILogger _logger;

        public static Logger Instance { get; } = new Logger();

        public Logger()
        {
            _logger = GetLogger();
        }

        private NLog.ILogger GetLogger(string loggerName = INFO)
        {
            return NLog.LogManager.GetLogger(loggerName);
        }

        public void Log(LogEntry entry)
        {
            _logger = GetLogger(INFO);
            switch (entry.Severity)
            {
                case LoggingEventType.Debug:
                    _logger.Debug(entry.Exception, entry.Message);
                    break;
                case LoggingEventType.Information:
                    _logger.Info(entry.Exception, entry.Message);
                    break;
                case LoggingEventType.Warning:
                    _logger.Warn(entry.Exception, entry.Message);
                    break;
                case LoggingEventType.Error:
                    _logger.Error(entry.Exception);
                    break;
                case LoggingEventType.Fatal:
                    _logger.Fatal(entry.Exception, entry.Message);
                    break;
                default:
                    break;
            }
        }
        public void Log(LogEntry entry, HttpContext httpContext)
        {
            _logger = GetLogger(EXCEPTION);
            var logEventInfo = new LogEventInfo(NLog.LogLevel.Error, "Error", entry.Exception.Message);
            logEventInfo.Properties.Add("TargetSite", entry.Exception.TargetSite);
            logEventInfo.Properties.Add("InnerException", entry.Exception.InnerException);
            logEventInfo.Properties.Add("Source", entry.Exception.Source);
            logEventInfo.Properties.Add("RequestedURL", httpContext.Request.Path);
            logEventInfo.Properties.Add("Method", httpContext.Request.Method);
            logEventInfo.Properties.Add("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
            logEventInfo.Properties.Add("Host", httpContext.Request.Host.ToString());
            logEventInfo.Properties.Add("ClientIP", httpContext.Connection.RemoteIpAddress);
            //   logEventInfo.Properties.Add("EmailSubject", $"{ConfigSettings.getAppSetting("ErrorMailSubject")} :  {entry.Exception.Message}");
            _logger.Error(entry.Exception, entry.Exception.Message, logEventInfo);

        }
        public void Log(HttpContext httpContext, string body = "")
        {
            _logger = GetLogger(INFO);
            httpContext.Request.EnableRewind();
            var logEventInfo = new LogEventInfo(NLog.LogLevel.Info, _logger.Name, string.Empty);
            logEventInfo.Properties.Add("Host", Environment.MachineName);
            logEventInfo.Properties.Add("RequestedURL", httpContext.Request.GetDisplayUrl().ToString());
            logEventInfo.Properties.Add("Method", httpContext.Request.Method.ToString());
            logEventInfo.Properties.Add("ClientIP", httpContext.Connection.RemoteIpAddress);
            logEventInfo.Properties.Add("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
            logEventInfo.Properties.Add("RequestBody", body);
            _logger.Log(logEventInfo);
        }
    }
}
