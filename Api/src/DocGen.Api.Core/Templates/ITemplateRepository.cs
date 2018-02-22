using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Api.Core.Templates
{
    public interface ITemplateRepository
    {
        Task<Template> CreateTemplateAsync(Template template);
    }
}
