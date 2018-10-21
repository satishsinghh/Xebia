using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xebia.CommonUtility.Logger;

namespace Xebia.Service.Host.Middleware
{
    public class ErrorLog
    {
        private readonly RequestDelegate next;
        protected static Logger logger = Logger.Instance;

        public ErrorLog(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                logger.Log(context, string.Empty);
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            logger.Log(exception, context);
            var statusCode = context.Features.Get<IExceptionHandlerFeature>()?.Error is HttpException httpEx ? httpEx.StatusCode : (HttpStatusCode)context.Response.StatusCode;
            var message = $"Message : {exception.Message} , Source : {exception.Source}, StackTrace : {exception.StackTrace}";
            var result = JsonConvert.SerializeObject(new { error = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorLogMiddlewareExtension
    {
        public static IApplicationBuilder UseErrorLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorLog>();
        }
    }

    public class HttpException : Exception
    {
        public HttpException(HttpStatusCode statusCode) { StatusCode = statusCode; }
        public HttpStatusCode StatusCode { get; private set; }
    }
}
