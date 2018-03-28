using DocGen.Shared.ModelEncoding;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core.Documents
{
    public class DocumentEncoder : IDocumentEncoder
    {
        private readonly IModelEncoderFactory _modelEncoderFactory;

        public DocumentEncoder(
            IModelEncoderFactory modelEncoderFactory)
        {
            _modelEncoderFactory = modelEncoderFactory;
        }

        public string Encode(DocumentCreate document)
        {
            return GetModelEncoder().Encode(new NoncedDocumentCreate()
            {
                TemplateId = document.TemplateId,
                TemplateVersion = document.TemplateVersion,
                InputValues = document.InputValues,
                Nonce = Guid.NewGuid()
            });
        }

        public DocumentCreate Decode(string encodedDocument)
        {
            var noncedDocument = GetModelEncoder().Decode(encodedDocument);
            return new DocumentCreate()
            {
                TemplateId = noncedDocument.TemplateId,
                TemplateVersion = noncedDocument.TemplateVersion,
                InputValues = noncedDocument.InputValues
            };
        }

        private IModelEncoder<NoncedDocumentCreate> GetModelEncoder() => _modelEncoderFactory.CreateModelEncoder<NoncedDocumentCreate>("TODO: Key");

        private class NoncedDocumentCreate : DocumentCreate
        {
            public Guid Nonce { get; set; }
        }
    }
}
