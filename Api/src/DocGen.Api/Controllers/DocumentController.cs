using DocGen.Api.Core.Documents;
using DocGen.Templating.Rendering;
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
        [ProducesResponseType(typeof(SerializableDocument_OLD), 200)]
        public async Task<IActionResult> Create([FromQuery] string templateId)
        {
            var documentCreate = new DocumentCreate()
            {
                TemplateId = templateId,
                TemplateVersion = 1, // TODO
                InputValues = Request.Query
                    .Where(kvp => kvp.Key != nameof(templateId))
                    .ToDictionary(kvp => kvp.Key, kvp => (dynamic)kvp.Value)
            };

            if (Request.ContentType == "text/plain")
            {
                var document = await _documentService.CreateTextDocumentAsync(documentCreate);
                return Content(document.Body);
            }
            else {
                var document = await _documentService.CreateSerializableDocumentAsync(documentCreate);
                return Ok(document);
            }
        }
    }
}
