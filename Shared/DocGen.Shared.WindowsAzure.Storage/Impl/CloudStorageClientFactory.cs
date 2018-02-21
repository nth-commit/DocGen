using System;
using System.Collections.Generic;
using System.Text;
using DocGen.Shared.Framework;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace DocGen.Shared.WindowsAzure.Storage.Impl
{
    class CloudStorageClientFactory : ICloudStorageClientFactory
    {
        private readonly IConfigurationProvider _configurationProvider;

        public CloudStorageClientFactory(
            IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public CloudBlobClient CreateBlobClient() => GetStorageAccount().CreateCloudBlobClient();

        public CloudTableClient CreateTableClient() => GetStorageAccount().CreateCloudTableClient();

        private CloudStorageAccount GetStorageAccount() => CloudStorageAccount.Parse(_configurationProvider.GetConnectionString(typeof(Marker)));
    }
}
