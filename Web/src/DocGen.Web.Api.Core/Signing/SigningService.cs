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
        private readonly ITemplateRepository _templateRepository;
        private readonly IDocumentExportsFactory _documentExportsFactory;
        private readonly ISigningRequestRepository _signingRequestRepository;
        private readonly ISignatureImageRepository _signatureImageRepository;
        private readonly ISigningNotifier _signingNotifier;

        public SigningService(
            IDocumentExportsFactory documentExportsFactory,
            ITemplateRepository templateRepository,
            ISigningRequestRepository signingRequestRepository,
            ISignatureImageRepository signatureImageRepository,
            ISigningNotifier signingNotifier)
        {
            _documentExportsFactory = documentExportsFactory;
            _templateRepository = templateRepository;
            _signingRequestRepository = signingRequestRepository;
            _signatureImageRepository = signatureImageRepository;
            _signingNotifier = signingNotifier;
        }

        public async Task<SigningRequestResult> CreateSigningRequestAsync(DocumentCreate document)
        {
            // TODO: Validate document
            var template = await _templateRepository.GetTemplateAsync(document.TemplateId);

            var signingRequest = new SigningRequest()
            {
                Id = Guid.NewGuid().ToString(),
                TemplateId = template.Id,
                TemplateVersion = template.Version,
                InputValues = document.InputValues,
                Signatories = _documentExportsFactory.Create(template, document.InputValues).ListSignatories()
            };

            await _signingRequestRepository.CreateSigningRequestAsync(signingRequest);

            var result = await _signingNotifier.NotifyAsync(signingRequest);
            return new SigningRequestResult()
            {
                Type = _signingNotifier.NotificationTypeId,
                TypeData = result
            };
        }
    }
}
