using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Api.Core.Documents
{
    public interface IDocumentRepository
    {
        Task<Document> CreateDocumentAsync(Document document);
    }
}
