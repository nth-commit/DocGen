using AutoMapper;
using DocGen.Api.Core.Templates;
using DocGen.Shared.Core.Dynamic;
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

            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<TemplateService>();

            try
            {
                service.CreateTemplateAsync(new TemplateCreate()
                {
                    Name = "Non-Disclosure Agreement",
                    Description = "Test",
                    Steps = new List<TemplateStepCreate>()
                    {
                        new TemplateStepCreate()
                        {
                            Id = "title1",
                            Name = "Title 1",
                            Description = "Desc 1",
                            Inputs = new List<TemplateStepInputCreate>()
                            {
                                new TemplateStepInputCreate()
                                {
                                    Type = TemplateStepInputType.Text,
                                    Hint = "Enter some text yo!"
                                }
                            }
                        },
                        new TemplateStepCreate()
                        {
                            Id = "title2",
                            Name = "Title 2",
                            Description = "Description 2",
                            Inputs = new List<TemplateStepInputCreate>()
                            {
                                new TemplateStepInputCreate()
                                {
                                    Type = TemplateStepInputType.Checkbox,
                                    Hint = "Some hint"
                                }
                            }
                        },
                        new TemplateStepCreate()
                        {
                            Id = "title3",
                            Name = "Title 3",
                            Description = "Description 3",
                            ConditionType = TemplateComponentConditionType.EqualsPreviousInputValue,
                            ConditionTypeData = ExpandoObjectFactory.CreateDynamic(new Dictionary<string, object>()
                            {
                                { "PreviousInputReference", "title2" },
                                { "PreviousInputValue", true }
                            }),
                            Inputs = new List<TemplateStepInputCreate>()
                            {
                                new TemplateStepInputCreate()
                                {
                                    Type = TemplateStepInputType.Text,
                                    Hint = "Enter some text yo!"
                                }
                            }
                        }
                    }
                },
                dryRun: true).GetAwaiter().GetResult();
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
