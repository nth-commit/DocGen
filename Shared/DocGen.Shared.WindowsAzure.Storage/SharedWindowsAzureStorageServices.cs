using DocGen.Shared.WindowsAzure.Storage;
using DocGen.Shared.WindowsAzure.Storage.Impl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SharedWindowsAzureStorageServices
    {
        public static IServiceCollection AddWindowsAzureStorageServices(
            this IServiceCollection services)
        {
            services.AddTransient<ICloudStorageClientFactory, CloudStorageClientFactory>();
            return services;
        }
    }
}
