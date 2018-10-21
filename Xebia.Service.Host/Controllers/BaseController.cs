using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Xebia.Service.Host.Filters;

namespace Xebia.Service.Host.Controllers
{
    [RequestLog]
    [EnableCors("CorsPolicy")]
    [Route("[controller]/[action]")]
    public class BaseController : Controller
    {
    }
}