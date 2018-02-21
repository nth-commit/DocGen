using DocGen.Api.Core.Templates;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoMapper
{
    public static class ApiCoreMappers
    {
        public static void AddApiCoreMappers(
            this IMapperConfigurationExpression conf)
        {
            conf.AddProfile<TemplateMappingProfile>();
        }
    }
}
