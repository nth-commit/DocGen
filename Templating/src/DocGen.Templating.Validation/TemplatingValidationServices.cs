using DocGen.Templating.Validation;
using DocGen.Templating.Validation.Shared;
using DocGen.Templating.Validation.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TemplatingValidationServices
    {
        public static IServiceCollection AddTemplatingValidationServices(
            this IServiceCollection services)
        {
            services.AddTransient<ITemplateMarkupValidator, TemplateMarkupValidator>();
            services.AddSingleton<ISchemaFileLocator>(new SchemaFileLocator(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)));

            // V1
            services.AddTransient<IVersionedTemplateMarkupValidator, TemplateMarkupValidatorV1>();

            return services;
        }
    }
}
