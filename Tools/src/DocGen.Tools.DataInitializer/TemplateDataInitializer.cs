using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocGen.Api.Core.Templates;
using DocGen.Shared.WindowsAzure.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DocGen.Tools.DataInitializer
{
    public class TemplateDataInitializer : IDataInitializer
    {
        private readonly TemplateService _templateService;
        private readonly ICloudStorageClientFactory _cloudStorageClientFactory;

        public TemplateDataInitializer(
            TemplateService templateService,
            ICloudStorageClientFactory cloudStorageClientFactory)
        {
            _templateService = templateService;
            _cloudStorageClientFactory = cloudStorageClientFactory;
        }

        public async Task RunAsync(ILogger logger)
        {
            var tableClient = _cloudStorageClientFactory.CreateTableClient();
            var templatesTable = tableClient.GetTableReference("templates");
            await templatesTable.DeleteIfExistsAsync();

            foreach (var file in Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Data", "Templates")))
            {
                var template = JsonConvert.DeserializeObject<TemplateCreate>(File.ReadAllText(file));
                await _templateService.CreateTemplateAsync(template);
            }
        }
    }
}
