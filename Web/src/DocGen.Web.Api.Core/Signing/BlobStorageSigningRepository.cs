using DocGen.Shared.WindowsAzure.Storage;
using DocGen.Web.Api.Core.Documents;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Web.Api.Core.Signing
{
    public class BlobStorageSigningRepository : ISigningRequestRepository, ISignatureImageRepository
    {
        private readonly ICloudStorageClientFactory _cloudStorageClientFactory;
        private readonly IDocumentEncoder _documentEncoder;

        public BlobStorageSigningRepository(
            ICloudStorageClientFactory cloudStorageClientFactory,
            IDocumentEncoder documentEncoder)
        {
            _cloudStorageClientFactory = cloudStorageClientFactory;
            _documentEncoder = documentEncoder;
        }

        public async Task CreateSigningRequestAsync(SigningRequest signingRequest)
        {
            var container = GetBlobContainer();

            var signingRequestId = GetSigningRequestId(signingRequest);
            var signingRequestDirectory = container.GetDirectoryReference(signingRequestId);

            var placeholderBlob = signingRequestDirectory.GetBlockBlobReference("created");
            await placeholderBlob.DeleteIfExistsAsync();
            await placeholderBlob.UploadTextAsync(string.Empty);
        }

        public async Task<bool> HasSigningRequestAsync(SigningRequest signingRequest)
        {
            var container = GetBlobContainer();

            var signingRequestId = GetSigningRequestId(signingRequest);
            var signingRequestDirectory = container.GetDirectoryReference(signingRequestId);

            var placeholderBlob = signingRequestDirectory.GetBlockBlobReference("created");
            return await placeholderBlob.ExistsAsync();
        }

        private CloudBlobContainer GetBlobContainer()
        {
            var client = _cloudStorageClientFactory.CreateBlobClient();

            var container = client.GetContainerReference("signings");
            container.CreateIfNotExistsAsync();

            return container;
        }

        private string GetSigningRequestId(SigningRequest signingRequest)
        {
            return _documentEncoder.Encode(new DocumentCreate()
            {
                TemplateId = signingRequest.TemplateId,
                TemplateVersion = signingRequest.TemplateVersion,
                InputValues = signingRequest.InputValues
            });
        }
    }
}
