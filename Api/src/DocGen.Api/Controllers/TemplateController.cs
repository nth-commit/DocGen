using DocGen.Api.Core.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocGen.Api.Controllers
{
    [Route("templates")]
    public class TemplateController : Controller
    {
        private readonly TemplateService _templateService;

        public TemplateController(
            TemplateService templateService)
        {
            _templateService = templateService;
        }

        [Authorize]
        [HttpGet("")]
        [ProducesResponseType(typeof(Template[]), 200)]
        public Task<IEnumerable<Template>> List() => _templateService.ListTemplatesAsync();

        [HttpGet("{id}")]
        public Task<Template> Get(string id) => _templateService.GetTemplateAsync(id);
    }
}
