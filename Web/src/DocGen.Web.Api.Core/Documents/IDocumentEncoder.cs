using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core.Documents
{
    public interface IDocumentEncoder
    {
        string Encode(DocumentCreate document, Guid? nonce = null);

        DocumentCreate Decode(string encodedDocument, out Guid nonce);
    }

    public static class DocumentEncoderExtensions
    {
        public static DocumentCreate Decode(this IDocumentEncoder documentEncoder, string encodedDocument)
        {
            Guid nonce;
            var result = documentEncoder.Decode(encodedDocument, out nonce);
            return result;
        }
    }
}
