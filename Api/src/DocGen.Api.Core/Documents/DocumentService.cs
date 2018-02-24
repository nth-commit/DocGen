using AutoMapper;
using MoreLinq;
using DocGen.Api.Core.Templates;
using DocGen.Shared.Framework;
using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Api.Core.Documents
{
    public class DocumentService
    {
        private readonly IMapper _mapper;
        private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;
        private readonly IDocumentRepository _documentRepository;
        private readonly ITemplateRepository _templateRepository;

        public DocumentService(
            IMapper mapper,
            IRemoteIpAddressAccessor remoteIpAddressAccessor,
            IDocumentRepository documentRepository,
            ITemplateRepository templateRepository)
        {
            _mapper = mapper;
            _remoteIpAddressAccessor = remoteIpAddressAccessor;
            _documentRepository = documentRepository;
            _templateRepository = templateRepository;
        }

        public async Task<Document> CreateDocumentAsync(DocumentCreate create)
        {
            Validator.ValidateNotNull(create, nameof(create));
            ValidateDocument(create);

            Template template = null;
            try
            {
                template = await _templateRepository.GetTemplateAsync(create.TemplateId);
            }
            catch (EntityNotFoundException ex)
            {
                ThrowEntityNotFoundAsClientModelValidation(ex, nameof(create.TemplateId));
            }

            //var stepsByStepGroupIndexByIndex = template.StepGroups
            //    .ToDictionary(
            //        (sg, i) => i,
            //        (sg, i) => sg.Steps.ToDictionary(
            //            (s, j) => j));

            //create.Values.ForEach(kvp1 =>
            //{
            //    kvp1.Value.ForEach(kvp2 =>
            //    {

            //    });
            //});

            var document = _mapper.Map<Document>(create);

            return document;
        }

        private void ValidateDocument(DocumentCreate create)
        {

        }

        private void ThrowEntityNotFoundAsClientModelValidation(EntityNotFoundException ex, string member)
        {
            throw new ClientModelValidationException($"{ex.Entity} with ID {ex.EntityId} was not found", member);
        }
    }
}
