using DocGen.Web.Shared.Signing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebSharedServices
    {
        public static IServiceCollection AddWebSharedServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSharedModelEncodingServices(configuration);
            services.AddTransient<ISigningKeyEncoder, SigningKeyEncoder>();
            return services;
        }
    }
}
