using AutoMapper;
using DocGen.Shared.Framework.SlugBuilder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public class TemplateMappingProfile : Profile
    {
        public TemplateMappingProfile()
        {
            CreateMap<TemplateCreate, Template>()
                .ForMember(dest => dest.Id, opts => opts.ResolveUsing(src => new SlugBuilderFactory().Create().Add(src.Name).ToString()));

            CreateMap<TemplateStepCreate, TemplateStep>()
                .ForMember(dest => dest.Inputs, opts => opts.ResolveUsing((src, dest, member, context)
                    => src.Inputs.Select(srcInput => context.Mapper.Map<TemplateStepInput>(srcInput, opts2 => opts2.Items["Step"] = src))));

            CreateMap<TemplateStepInputCreate, TemplateStepInput>();
                //.ForMember(dest => dest.Id, opts => opts.ResolveUsing((src, dest, member, context)
                //    => TemplateIdResolver.Instance.ResolveStepInputId(context.Items["Step"] as TemplateStepCreate, src)));

            CreateMap<Template, TemplateTableEntity>()
                .ForMember(dest => dest.RowKey, opts => opts.ResolveUsing(src => src.Id))
                .ForMember(dest => dest.PartitionKey, opts => opts.ResolveUsing(src => Regions.Constants.Names.NewZealand))
                .ForMember(dest => dest.DataJson, opts => opts.ResolveUsing(src => JsonConvert.SerializeObject(src)));

            CreateMap<TemplateTableEntity, Template>()
                .ConvertUsing((src, dest) =>
                {

                    return JsonConvert.DeserializeObject<Template>(src.DataJson);
                });
        }
    }
}
