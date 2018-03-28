using DocGen.Shared.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core.Signing
{
    public class BlobStorageSignatureImageRepository : ISignatureImageRepository
    {
        private readonly ICloudStorageClientFactory _cloudStorageClientFactory;

        public BlobStorageSignatureImageRepository(
            ICloudStorageClientFactory cloudStorageClientFactory)
        {
            _cloudStorageClientFactory = cloudStorageClientFactory;
        }

        private CloudBlobContainer GetBlobContainer()
        {
            var client = _cloudStorageClientFactory.CreateBlobClient();

            var container = client.GetContainerReference("signings");
            container.CreateIfNotExistsAsync();

            return container;
        }
    }
}
