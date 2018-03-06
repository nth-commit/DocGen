using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Api.Core.Templates
{
    public interface ITemplateRepository
    {
        Task<IEnumerable<Template>> ListTemplatesAsync();

        Task<Template> GetTemplateAsync(string id);

        Task<Template> CreateTemplateAsync(Template template);
    }
}
