using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DocGen.Api.Core.Templates;

namespace DocGen.Api.Core.Documents
{
    public class RawTextDocumentGenerator : IDocumentGenerator
    {
        public DocumentGenerationMode GenerationMode => DocumentGenerationMode.RawText;

        public Task<string> GenerateAsync(Template template)
        {
            throw new NotImplementedException();
        }
    }
}
