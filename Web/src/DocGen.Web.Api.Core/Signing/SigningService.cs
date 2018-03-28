using DocGen.Shared.Framework;
using DocGen.Shared.Validation;
using DocGen.Web.Api.Core.Documents;
using DocGen.Web.Api.Core.Templates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Web.Api.Core.Signing
{
    public class SigningService
    {
        private readonly IDocumentEncoder _documentEncoder;
        private readonly ITemplateRepository _templateRepository;
        private readonly IDocumentExportsFactory _documentExportsFactory;
        private readonly ISigningRequestRepository _signingRequestRepository;
        private readonly ISignatureImageRepository _signatureImageRepository;

        public SigningService(
            IDocumentEncoder documentEncoder,
            IDocumentExportsFactory documentExportsFactory,
            ITemplateRepository templateRepository,
            ISigningRequestRepository signingRequestRepository,
            ISignatureImageRepository signatureImageRepository)
        {
            _documentEncoder = documentEncoder;
            _documentExportsFactory = documentExportsFactory;
            _templateRepository = templateRepository;
            _signingRequestRepository = signingRequestRepository;
            _signatureImageRepository = signatureImageRepository;
        }

        public async Task CreateSigningRequestAsync(string documentEncoded)
        {
            Guid nonce;
            var document = _documentEncoder.Decode(documentEncoded, out nonce);
            var template = await _templateRepository.GetTemplateAsync(document.TemplateId);

            var signingRequest = new SigningRequest()
            {
                Nonce = nonce,
                TemplateId = template.Id,
                TemplateVersion = template.Version,
                InputValues = document.InputValues,
                Signatories = _documentExportsFactory.Create(template, document.InputValues).ListSignatories()
            };
            await ValidateSigningRequestUnique(signingRequest);

            await _signingRequestRepository.CreateSigningRequestAsync(signingRequest);
        }

        private async Task ValidateSigningRequestUnique(SigningRequest signingRequest)
        {
            if (await _signingRequestRepository.HasSigningRequestAsync(signingRequest))
            {
                throw new ClientValidationException("A signing request already exists for this document");
            }
        }

    }
}
