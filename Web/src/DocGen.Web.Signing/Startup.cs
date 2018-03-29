using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocGen.Web.Shared.Signing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DocGen.Web.Signing
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            var builder = new ConfigurationBuilder()
                .AddFrameworkConfigurationSources(_hostingEnvironment.EnvironmentName);

            _configuration = builder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFrameworkServices(_configuration);
            services.AddWebSharedServices(_configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ISigningKeyEncoder signingKeyEncoder,
            IOptions<HostOptions> hostOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(context =>
            {
                var encodedSigningKey = context.Request.Path.Value.Substring(1);
                var signingKey = signingKeyEncoder.Decode(encodedSigningKey);

                // TODO: Audit

                var appBase = new Uri(hostOptions.Value.App);

                var uriBuilder = new UriBuilder();
                uriBuilder.Host = appBase.Host;
                uriBuilder.Scheme = appBase.Scheme;
                uriBuilder.Port = appBase.Port;
                uriBuilder.Path = "sign";
                uriBuilder.Query = $"email={signingKey.SignatoryEmail}&key={encodedSigningKey}";

                var redirectUrl = uriBuilder.ToString();
                context.Response.ContentType = "text/html";
                context.Response.WriteAsync($"<html><head><script>window.location.replace('{redirectUrl}');</script></head><body></body></html>");

                return Task.CompletedTask;
            });
        }
    }
}
