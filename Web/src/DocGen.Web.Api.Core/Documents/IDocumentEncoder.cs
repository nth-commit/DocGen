using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core.Documents
{
    public interface IDocumentEncoder
    {
        string Encode(DocumentCreate document);

        DocumentCreate Decode(string encodedDocument);
    }
}
