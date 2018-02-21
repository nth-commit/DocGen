using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SharedFrameworkServices
    {
        public static IServiceCollection AddConfigurationProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<DocGen.Shared.Framework.IConfigurationProvider>(new DocGen.Shared.Framework.Impl.ConfigurationProvider(configuration));
            return services;
        }
    }
}
