using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Api.Core.Templates
{
    public interface ITemplateService
    {
        Task<Template> CreateTemplateAsync(Template template, bool dryRun = false);
    }
}
