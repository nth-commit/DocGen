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

        public string Encode(DocumentCreate document, Guid? nonce = null)
        {
            return GetModelEncoder().Encode(new NoncedDocumentCreate()
            {
                TemplateId = document.TemplateId,
                TemplateVersion = document.TemplateVersion,
                InputValues = document.InputValues,
                Nonce = nonce ?? Guid.NewGuid()
            });
        }

        public DocumentCreate Decode(string encodedDocument, out Guid nonce)
        {
            var noncedDocument = GetModelEncoder().Decode(encodedDocument);
            nonce = noncedDocument.Nonce;
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
