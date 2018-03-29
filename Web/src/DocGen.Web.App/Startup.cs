using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DocGen.Web.App
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            var builder = new ConfigurationBuilder()
                .AddFrameworkConfigurationSources(_hostingEnvironment.ApplicationName);

            _configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                services.AddCors();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole();

            if (!env.IsDevelopment())
            {
                app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors(opts => opts
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials());

                app.Use(async (context, next) =>
                {
                    if (context.Request.Path == "/config")
                    {
                        context.Response.ContentType = "application/json";
                        var envSettings = GetEnvironmentSettingsJson(context);
                        await context.Response.WriteAsync(envSettings);
                    }
                    else
                    {
                        await next.Invoke();
                    }
                });
            }
            else
            {
                app.UseStaticFiles();

                app.Run(context =>
                {
                    context.Response.ContentType = "text/html";

                    using (var fs = File.OpenRead("wwwroot/index.html"))
                    {
                        var doc = new HtmlDocument();
                        doc.Load(fs);

                        var appSettingsScript = doc.CreateElement("script");
                        appSettingsScript.AppendChild(doc.CreateTextNode($"var DOCGEN_ENVIRONMENT_SETTINGS = {GetEnvironmentSettingsJson(context)}"));

                        var head = doc.DocumentNode.SelectSingleNode("/html/head");
                        head.AppendChild(appSettingsScript);

                        doc.Save(context.Response.Body);
                    }

                    return Task.FromResult(0);
                });
            }
        }

        private string GetEnvironmentSettingsJson(HttpContext context)
        {
            var environmentSettings = new
            {
                Urls = new
                {
                    Api = _configuration["Hosts:Api"]
                }
            };
            return JsonConvert.SerializeObject(environmentSettings);
        }
    }
}
