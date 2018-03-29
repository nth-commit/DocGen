using DocGen.Shared.ModelEncoding;
using DocGen.Shared.WindowsAzure.Storage;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SharedModelEncodingServices
    {
        public static IServiceCollection AddSharedModelEncodingServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var container = GetKeyBlobContainer(configuration);
            services.AddDataProtection(opts => opts.ApplicationDiscriminator = "DocGen.Shared.ModelEncoding")
                .PersistKeysToAzureBlobStorage(container, "keys.xml");

            services.AddTransient<IModelEncoderFactory, ProtectedModelEncoderFactory>();

            return services;
        }

        private static CloudBlobContainer GetKeyBlobContainer(IConfiguration configuration)
        {
            var services = new ServiceCollection();
            services.AddFrameworkServices(configuration);
            services.AddWindowsAzureStorageServices();

            var serviceProvider = services.BuildServiceProvider();

            var cloudClientFactory = serviceProvider.GetRequiredService<ICloudStorageClientFactory>();
            var client = cloudClientFactory.CreateBlobClient();

            var container = client.GetContainerReference("dataprotection");
            container.CreateIfNotExistsAsync().GetAwaiter().GetResult();

            return container;
        }
    }
}
