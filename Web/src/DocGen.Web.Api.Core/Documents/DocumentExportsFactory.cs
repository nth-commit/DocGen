using System;
using System.Collections.Generic;
using System.Text;
using DocGen.Templating.Rendering;
using DocGen.Web.Api.Core.Templates;
using MoreLinq;

namespace DocGen.Web.Api.Core.Documents
{
    public class DocumentExportsFactory : IDocumentExportsFactory
    {
        public DocumentExports Create(Template template, Dictionary<string, dynamic> inputValues)
        {
            var result = new DocumentExports();

            inputValues.ForEach(kvp =>
            {
                var input = template.GetInputById(kvp.Key);
                if (!string.IsNullOrEmpty(input.ExportAs))
                {
                    result.Add(input.ExportAs, ((object)kvp.Value).ToString());
                }
            });

            return result;
        }
    }
}
