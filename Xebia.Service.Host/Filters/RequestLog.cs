using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xebia.CommonUtility.Logger;

namespace Xebia.Service.Host.Filters
{
    
    public class RequestLog : Attribute, IActionFilter
    {
        protected static Logger logger = Logger.Instance;
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var requestURL = context.HttpContext.Request.GetDisplayUrl();
            foreach (ControllerParameterDescriptor param in context.ActionDescriptor.Parameters)
            {
                if (param.ParameterInfo.CustomAttributes.Any(attr => attr.AttributeType == typeof(FromBodyAttribute)))
                {
                    if (context.ActionArguments[param.Name] != null)
                    {
                        string body = JsonConvert.SerializeObject(context.ActionArguments[param.Name]);
                        logger.Log(context.HttpContext, body);
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //To do : after the action executes  
        }
    }
}
