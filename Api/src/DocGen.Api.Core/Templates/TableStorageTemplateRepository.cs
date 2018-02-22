using AutoMapper;
using DocGen.Shared.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Api.Core.Templates
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

        public async Task<Template> CreateTemplateAsync(Template template)
        {
            var templateRow = _mapper.Map<TemplateTableEntity>(template);

            var tableClient = _cloudStorageClientFactory.CreateTableClient();

            var table = tableClient.GetTableReference("templates");
            await table.CreateIfNotExistsAsync();

            await table.ExecuteAsync(TableOperation.Insert(templateRow));

            return template;
        }
    }
}
