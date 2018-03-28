using DocGen.Web.Api.Core.Templates;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core.Documents
{
    public interface IDocumentExportsFactory
    {
        DocumentExports Create(Template template, Dictionary<string, dynamic> inputValues);
    }
}
