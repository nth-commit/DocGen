using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SharedFrameworkServices
    {
        public static IServiceCollection AddFrameworkServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<DocGen.Shared.Framework.IConfigurationProvider>(new DocGen.Shared.Framework.Impl.ConfigurationProvider(configuration));

            services.AddOptions();
            services.Configure<HostOptions>(configuration.GetSection("Hosts"));

            return services;
        }
    }

    public class HostOptions
    {
        public string Api { get; set; }
        public string Signing { get; set; }
    }
}
