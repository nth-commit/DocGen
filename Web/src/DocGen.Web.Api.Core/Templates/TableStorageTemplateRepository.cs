using AutoMapper;
using DocGen.Shared.Framework;
using DocGen.Shared.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Web.Api.Core.Templates
{
    public class TableStorageTemplateRepository : ITemplateRepository
    {
        private readonly ICloudStorageClientFactory _cloudStorageClientFactory;
        private readonly IMapper _mapper;

        public TableStorageTemplateRepository(
            ICloudStorageClientFactory cloudStorageClientFactory,
            IMapper mapper)
        {
            _cloudStorageClientFactory = cloudStorageClientFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Template>> ListTemplatesAsync()
        {
            var table = await GetTableReferenceAsync();

            var continuationToken = new TableContinuationToken();
            var templateTableEntities = await table.ExecuteQuerySegmentedAsync(new TableQuery<TemplateTableEntity>(), continuationToken);

            return templateTableEntities.Results.Select(t => _mapper.Map<Template>(t));
        }

        public async Task<Template> GetTemplateAsync(string id)
        {
            var table = await GetTableReferenceAsync();

            var retrieveResult = await table.ExecuteAsync(TableOperation.Retrieve<TemplateTableEntity>(Regions.Constants.Names.NewZealand, id));

            if (retrieveResult.Result == null)
            {
                throw new EntityNotFoundException("Template", id);
            }
            else
            {
                return _mapper.Map<Template>(retrieveResult.Result);
            }
        }

        public async Task<Template> CreateTemplateAsync(Template template)
        {
            var templateTableEntity = _mapper.Map<TemplateTableEntity>(template);

            var table = await GetTableReferenceAsync();

            await table.ExecuteAsync(TableOperation.Insert(templateTableEntity));

            return template;
        }

        private async Task<CloudTable> GetTableReferenceAsync()
        {
            var tableClient = _cloudStorageClientFactory.CreateTableClient();

            var table = tableClient.GetTableReference("templates");
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}
