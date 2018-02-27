using DocGen.Api.Core.Templates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Api.Core.Documents
{
    public interface IDocumentGenerator
    {
        DocumentGenerationMode GenerationMode { get; }

        Task<string> GenerateAsync(Template template);
    }
}
