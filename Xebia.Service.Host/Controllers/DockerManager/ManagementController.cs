using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xebia.Service.Host.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Xebia.Service.Host.Controllers.DockerManager
{

    public class ManagementController : BaseController
    {

        [HttpGet]
        [Route("_health")]
        public IActionResult Get()
        {
            var message = $"Xebia Service OK\r\nEnvironment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "none"}\r\n";

            return Ok(message);
        }

        [HttpGet]
        [Route("_buildinfo")]
        public IActionResult BuildInfo()
        {
            var filedata = System.IO.File.ReadAllBytes(".buildinfo");
            return this.File(filedata, "text/plain");
        }
    }
}
