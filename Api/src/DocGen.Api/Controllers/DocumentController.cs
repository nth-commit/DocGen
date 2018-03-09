using DocGen.Api.Core.Documents;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocGen.Api.Controllers
{
    [Route("documents")]
    public class DocumentController : Controller
    {
        private readonly DocumentService _documentService;

        public DocumentController(
            DocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost("")]
        [Produces("text/plain")]
        public async Task<ContentResult> Create([FromQuery] string templateId)
        {
            var document = await _documentService.CreateDocumentAsync(new DocumentCreate()
            {
                TemplateId = templateId,
                TemplateVersion = 1, // TODO
                InputValues = Request.Query
                    .Where(kvp => kvp.Key != nameof(templateId))
                    .ToDictionary(kvp => kvp.Key, kvp => (dynamic)kvp.Value)
            });

            return Content(document.Body);
        }
    }
}
