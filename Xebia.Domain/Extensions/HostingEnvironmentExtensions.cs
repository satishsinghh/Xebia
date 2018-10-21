using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.DatabaseCore.Extensions
{
    public static class HostingEnvironmentExtensions
    {
        public static bool IsLocal(this IHostingEnvironment env)
        {
            return env.IsEnvironment("Development");
        }

        public static bool IsDev(this IHostingEnvironment env)
        {
            return env.IsEnvironment("Dev");
        }

        public static bool IsQA(this IHostingEnvironment env)
        {
            return env.IsEnvironment("QA");
        }

        public static bool IsStaging(this IHostingEnvironment env)
        {
            return env.IsEnvironment("Staging");
        }

        public static bool IsProd(this IHostingEnvironment env)
        {
            return env.IsEnvironment("Prod");
        }
    }
}
