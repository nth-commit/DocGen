using AutoMapper;
using DocGen.Api.Core.Documents;
using DocGen.Api.Core.Templates;
using DocGen.Shared.Core.Dynamic;
using DocGen.Shared.Framework;
using DocGen.Shared.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace DocGen.Tools.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddConfigurationProvider(configuration);
            services.AddWindowsAzureStorageServices();
            services.AddApiCoreServices();
            services.AddAutoMapper(conf =>
            {
                conf.AddApiCoreMappers();
            });

            services.AddTransient<IRemoteIpAddressAccessor, StubbedRemoteIpAddressAccessor>();

            var serviceProvider = services.BuildServiceProvider();

            try
            {
                #region Create template
                //var service = serviceProvider.GetRequiredService<TemplateService>();
                //service.CreateTemplateAsync(new TemplateCreate()
                //{
                //    Name = "Non-Disclosure Agreement",
                //    Description = "Test",
                //    Steps = new List<TemplateStepCreate>()
                //    {
                //        new TemplateStepCreate()
                //        {
                //            Id = "title1",
                //            Name = "Title 1",
                //            Description = "Desc 1",
                //            Inputs = new List<TemplateStepInputCreate>()
                //            {
                //                new TemplateStepInputCreate()
                //                {
                //                    Type = TemplateStepInputType.Text,
                //                    Hint = "Enter some text yo!"
                //                }
                //            }
                //        },
                //        new TemplateStepCreate()
                //        {
                //            Id = "title2",
                //            Name = "Title 2",
                //            Description = "Description 2",
                //            Inputs = new List<TemplateStepInputCreate>()
                //            {
                //                new TemplateStepInputCreate()
                //                {
                //                    Type = TemplateStepInputType.Checkbox,
                //                    Hint = "Some hint"
                //                }
                //            }
                //        },
                //        new TemplateStepCreate()
                //        {
                //            Id = "title3",
                //            Name = "Title 3",
                //            Description = "Description 3",
                //            ConditionType = TemplateComponentConditionType.EqualsPreviousInputValue,
                //            ConditionTypeData = ExpandoObjectFactory.CreateDynamic(new Dictionary<string, object>()
                //            {
                //                { "PreviousInputReference", "title2" },
                //                { "PreviousInputValue", true }
                //            }),
                //            Inputs = new List<TemplateStepInputCreate>()
                //            {
                //                new TemplateStepInputCreate()
                //                {
                //                    Type = TemplateStepInputType.Text,
                //                    Hint = "Enter some text yo!"
                //                }
                //            }
                //        }
                //    }
                //},
                //dryRun: true).GetAwaiter().GetResult();
                #endregion

                #region Create document

                var service = serviceProvider.GetRequiredService<DocumentService>();
                service.CreateDocumentAsync(
                    new DocumentCreate()
                    {
                        TemplateId = "non-disclosure-agreement",
                        TemplateVersion = 1,
                        InputValues = new Dictionary<string, dynamic>()
                        {
                            { "organisation.name", "Automio Limited" },
                            { "organisation.location", "New Plymouth" },
                            { "contractor.type", "company" },
                            { "contractor.company.name", "Lava Lamps Limited" },
                            { "contractor.company.location", "New Plymouth" },
                            { "diclosure_reason", "To provide marketing services to the Organisation" },
                            { "disclosure_access", true },
                            { "disclosure_access.details.persons", "sub-contractors, board members" }
                        }
                    },
                    DocumentGenerationMode.RawText).GetAwaiter().GetResult();

                #endregion
            }
            catch (ClientModelValidationException ex)
            {
                ex.ModelErrors.ForEach(kvp =>
                {
                    kvp.Value.ForEach(error =>
                    {
                        Console.WriteLine($"{kvp.Key}: {error}");
                    });
                });
                Console.ReadLine();
            }
        }
    }
}
