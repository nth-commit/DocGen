using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Api.Core.Documents
{
    public class DocumentMappingProfile : Profile
    {
        public DocumentMappingProfile()
        {
            CreateMap<DocumentCreate, Document>()
                .ForMember(dest => dest.Id, opts => opts.ResolveUsing(src => Guid.NewGuid()));


        }
    }
}
