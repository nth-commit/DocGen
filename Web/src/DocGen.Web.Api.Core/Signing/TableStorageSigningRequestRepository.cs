using AutoMapper;
using DocGen.Shared.Framework;
using DocGen.Shared.WindowsAzure.Storage;
using DocGen.Web.Api.Core.Documents;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Web.Api.Core.Signing
{
    public class TableStorageSigningRequestRepository : ISigningRequestRepository
    {
        private readonly ICloudStorageClientFactory _cloudStorageClientFactory;
        private readonly IMapper _mapper;

        public TableStorageSigningRequestRepository(
            ICloudStorageClientFactory cloudStorageClientFactory,
            IMapper mapper)
        {
            _cloudStorageClientFactory = cloudStorageClientFactory;
            _mapper = mapper;
        }

        public async Task<SigningRequest> GetSigningRequestAsync(string id)
        {
            var table = await GetTableReferenceAsync();

            var retrieveResult = await table.ExecuteAsync(
                TableOperation.Retrieve<SigningRequestTableEntity>(Regions.Constants.Names.NewZealand, id));

            if (retrieveResult.Result == null)
            {
                throw new EntityNotFoundException("SigningRequest", id);
            }
            else
            {
                return _mapper.Map<SigningRequest>(retrieveResult.Result);
            }
        }

        public async Task<SigningRequest> CreateSigningRequestAsync(SigningRequest signingRequest)
        {
            var signingRequestTableEntity = _mapper.Map<SigningRequestTableEntity>(signingRequest);

            var table = await GetTableReferenceAsync();

            await table.ExecuteAsync(TableOperation.Insert(signingRequestTableEntity));

            return signingRequest;
        }

        private async Task<CloudTable> GetTableReferenceAsync()
        {
            var client = _cloudStorageClientFactory.CreateTableClient();

            var table = client.GetTableReference("signings");
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}
