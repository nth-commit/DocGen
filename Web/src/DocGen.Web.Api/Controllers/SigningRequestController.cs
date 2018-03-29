using DocGen.Web.Api.Core.Documents;
using DocGen.Web.Api.Core.Signing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocGen.Web.Api.Controllers
{
    [Route("signingrequests")]
    public class SigningRequestController : Controller
    {
        private readonly SigningService _signingService;

        public SigningRequestController(
            SigningService signingService)
        {
            _signingService = signingService;
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(SigningRequestResult), 200)]
        public async Task<IActionResult> Create(
            [FromQuery] string templateId,
            [FromQuery] string templateVersion)
        {
            var document = GetDocumentCreate(templateId, templateVersion);
            var result = await _signingService.CreateSigningRequestAsync(document);
            return Ok(result);
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
