using Autofac;
using AutoMapper;
using DocGen.Api.Core.Templates;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Api.Core
{
    public class TestsBase : IDisposable
    {
        public TestsBase()
        {
            var services = new ServiceCollection();

            services.AddAutoMapper(conf =>
            {
                conf.AddApiCoreMappers();
            });

            services.AddApiCoreServices();
            services.AddTransient<ITemplateRepository, InMemoryTemplateRepository>();

            LifetimeScope = services.BuildAutofacServiceProvider()
                .GetRequiredService<ILifetimeScope>()
                .BeginLifetimeScope();
        }

        protected ILifetimeScope LifetimeScope { get; private set; }

        protected IServiceProvider ServiceProvider => LifetimeScope.Resolve<IServiceProvider>();

        public void Dispose()
        {
            LifetimeScope.Dispose();
        }
    }
}
