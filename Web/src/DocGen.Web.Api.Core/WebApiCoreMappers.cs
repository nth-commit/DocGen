using DocGen.Web.Api.Core.Documents;
using DocGen.Web.Api.Core.Templates;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoMapper
{
    public static class WebApiCoreMappers
    {
        public static void AddApiCoreMappers(
            this IMapperConfigurationExpression conf)
        {
            conf.AddProfile<DocumentMappingProfile>();
            conf.AddProfile<TemplateMappingProfile>();
        }
    }
}
