using DocGen.Shared.ModelEncoding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SharedModelEncodingServices
    {
        public static IServiceCollection AddSharedModelEncodingServices(
            this IServiceCollection services)
        {
            services.AddDataProtection();
            services.AddTransient<IModelEncoderFactory, ProtectedModelEncoderFactory>();
            return services;
        }
    }
}
