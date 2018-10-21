using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.DatabaseCore.Configuration
{
    public static class XebiaConfigProviderExtensions
    {
        public static IConfigurationBuilder AddEncryptedProvider(this IConfigurationBuilder builder, string path)
        {
            return builder.Add(new XebiaConfigProvider(path));
        }
    }
}
