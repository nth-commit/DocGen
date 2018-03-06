using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Api.Core.Templates
{
    public class InMemoryTemplateRepository : ITemplateRepository
    {
        private Dictionary<string, Template> _templatesById = new Dictionary<string, Template>();

        public Task<Template> CreateTemplateAsync(Template template)
        {
            _templatesById.Add(Guid.NewGuid().ToString(), template);
            return Task.FromResult(template);
        }

        public Task<Template> GetTemplateAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Template>> ListTemplatesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
