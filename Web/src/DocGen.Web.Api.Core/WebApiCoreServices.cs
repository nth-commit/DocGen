using DocGen.Web.Api.Core.Documents;
using DocGen.Web.Api.Core.Signing;
using DocGen.Web.Api.Core.Templates;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiCoreServices
    {
        public static IServiceCollection AddApiCoreServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddWebSharedServices(configuration);

            services.AddTransient<TemplateService>();
            services.AddTransient<ITemplateRepository, TableStorageTemplateRepository>();

            services.AddTransient<DocumentService>();
            services.AddTransient<IDocumentEncoder, DocumentEncoder>();
            services.AddTransient<IDocumentExportsFactory, DocumentExportsFactory>();

            services.AddTransient<SigningService>();
            services.AddTransient<ISigningRequestRepository, TableStorageSigningRequestRepository>();
            services.AddTransient<ISignatureImageRepository, BlobStorageSignatureImageRepository>();
            services.AddTransient<ISigningNotifier, StubbedSigningNotifier>();

            return services;
        }
    }
}
