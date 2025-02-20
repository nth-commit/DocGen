﻿using DocGen.Web.Api.Core.Templates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocGen.Web.Api.Controllers
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

        [HttpGet("")]
        [ProducesResponseType(typeof(Template[]), 200)]
        public async Task<IActionResult> List() => Ok(await _templateService.ListTemplatesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id) => Ok(await _templateService.GetTemplateAsync(id));
    }
}
