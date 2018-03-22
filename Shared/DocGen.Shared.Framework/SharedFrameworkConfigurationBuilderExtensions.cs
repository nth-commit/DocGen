using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddFrameworkConfigurationSources(
            this IConfigurationBuilder builder,
            string environmentName)
        {
            builder
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddJsonFile("appsettings.Shared.json", optional: false)
                .AddJsonFile($"appsettings.Shared.{environmentName}.json", optional: true);

            if (environmentName == "Development")
            {
                builder.AddUserSecrets("DocGen");
            }

            builder.AddEnvironmentVariables();

            return builder;
        }
    }
}
