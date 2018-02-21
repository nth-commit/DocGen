using AutoMapper;
using DocGen.Shared.Validation;
using DocGen.Shared.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Api.Core.Templates
{
    public class TemplateService : ITemplateService
    {
        private readonly ICloudStorageClientFactory _cloudStorageClientFactory;
        private readonly IMapper _mapper;

        public TemplateService(
            ICloudStorageClientFactory cloudStorageClientFactory,
            IMapper mapper)
        {
            _cloudStorageClientFactory = cloudStorageClientFactory;
            _mapper = mapper;
        }

        async Task<Template> ITemplateService.CreateTemplateAsync(Template template, bool dryRun)
        {
            Validator.ValidateNotNull(template, nameof(template));
            Validator.Validate(template);

            var templateRow = _mapper.Map<TemplateTableEntity>(template);

            var tableClient = _cloudStorageClientFactory.CreateTableClient();

            var table = tableClient.GetTableReference("templates");
            await table.CreateIfNotExistsAsync();

            await table.ExecuteAsync(TableOperation.Insert(templateRow));

            return template;
        }
    }
}
