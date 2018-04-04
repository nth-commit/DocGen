using DocGen.Web.Api.Core.Documents;
using DocGen.Templating.Rendering;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection;

namespace DocGen.Web.Api.Controllers
{
    [Route("documents")]
    public class DocumentController : Controller
    {
        private readonly DocumentService _documentService;
        private readonly IDocumentEncoder _documentEncoder;

        public DocumentController(
            DocumentService documentService,
            IDocumentEncoder documentEncoder)
        {
            _documentService = documentService;
            _documentEncoder = documentEncoder;
        }

        [HttpPost("")]
        [HttpGet("")]
        [ProducesResponseType(typeof(SerializableDocument), 200)]
        public async Task<IActionResult> Create(
            [FromQuery] string templateId,
            [FromQuery] string templateVersion,
            [FromQuery] string key)
        {
            DocumentCreate create = null;
            if (!string.IsNullOrEmpty(key))
            {
                create = _documentEncoder.Decode(key);
            }
            else if (!string.IsNullOrEmpty(templateId) && !string.IsNullOrEmpty(templateVersion))
            {
                create = GetDocumentCreate(templateId, templateVersion);
            }
            else
            {
                return BadRequest();
            }

            if (Request.ContentType == "text/plain")
            {
                var document = await _documentService.CreateTextDocumentAsync(create);
                return Content(document.Body);
            }
            else if (Request.ContentType == "application/vnd+document+key")
            {
                var document = _documentEncoder.Encode(create);
                return Content(document);
            }
            else if (Request.ContentType == "application/vnd+document+html")
            {
                var document = await _documentService.CreateHtmlDocumentAsync(create);
                return Ok(document);
            }
            else if (Request.ContentType == "application/vnd+document")
            {
                var document = await _documentService.CreateSerializableDocumentAsync(create);
                return Ok(document);
            }
            else
            {
                var document = await _documentService.CreateHtmlDocumentAsync(create);

                var html = @"
                    <html>
                        <head>
                            <style type=""text/css"">" + document.Css + @"</style>
                        </head>
                        <body>" + string.Join(string.Empty, document.Pages) + @"</body>
                    </html>";

                Response.ContentType = "text/html; charset=utf-8";

                return Content(html);
            }
        }

        private DocumentCreate GetDocumentCreate(string templateId, string templateVersion)
        {
            return new DocumentCreate()
            {
                TemplateId = templateId,
                TemplateVersion = int.Parse(templateVersion),
                InputValues = GetInputValues()
            };
        }

        private Dictionary<string, dynamic> GetInputValues()
        {
            return Request.Query
                .Where(kvp => kvp.Key.StartsWith("v_"))
                .ToDictionary(
                    kvp => kvp.Key.Substring("v_".Count()),
                    kvp => (dynamic)kvp.Value);
        }
    }
}
