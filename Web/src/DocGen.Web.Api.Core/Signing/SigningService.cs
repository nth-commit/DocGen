using DocGen.Shared.Framework;
using DocGen.Shared.Validation;
using DocGen.Templating.Rendering;
using DocGen.Web.Api.Core.Documents;
using DocGen.Web.Api.Core.Templates;
using DocGen.Web.Shared.Signing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Web.Api.Core.Signing
{
    public class SigningService
    {
        private readonly ISigningKeyEncoder _signingKeyEncoder;
        private readonly ITemplateRepository _templateRepository;
        private readonly IDocumentExportsFactory _documentExportsFactory;
        private readonly ISigningRequestRepository _signingRequestRepository;
        private readonly ISignatureImageRepository _signatureImageRepository;
        private readonly ISigningNotifier _signingNotifier;
        private readonly HostOptions _hostOptions;

        public SigningService(
            ISigningKeyEncoder signingKeyEncoder,
            IDocumentExportsFactory documentExportsFactory,
            ITemplateRepository templateRepository,
            ISigningRequestRepository signingRequestRepository,
            ISignatureImageRepository signatureImageRepository,
            ISigningNotifier signingNotifier,
            IOptions<HostOptions> hostOptions)
        {
            _signingKeyEncoder = signingKeyEncoder;
            _documentExportsFactory = documentExportsFactory;
            _templateRepository = templateRepository;
            _signingRequestRepository = signingRequestRepository;
            _signatureImageRepository = signatureImageRepository;
            _signingNotifier = signingNotifier;
            _hostOptions = hostOptions.Value;
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
                Signatories = _documentExportsFactory
                    .Create(template, document.InputValues)
                    .ListSignatories()
                    .Select(s => s.Id)
            };

            await _signingRequestRepository.CreateSigningRequestAsync(signingRequest);

            var signingKeysDecodedByEmail = signingRequest.Signatories.ToDictionary(
                email => email,
                email => new SigningKey()
                {
                    SignatoryEmail = email,
                    SigningRequestId = signingRequest.Id
                });

            var signingUrlsByEmail = signingKeysDecodedByEmail.ToDictionary(
                kvp => kvp.Key,
                kvp => $"{_hostOptions.Signing}/{_signingKeyEncoder.Encode(kvp.Value)}?v=1");

            await _signingNotifier.NotifyAsync(signingUrlsByEmail);

            return new SigningRequestResult()
            {
                Type = _signingNotifier.NotificationTypeId,
                TypeData = signingUrlsByEmail
            };
        }
    }
}
