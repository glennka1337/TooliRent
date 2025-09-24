using AutoMapper;
using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Mapping
{
    public class ToolProfile : Profile
    {
        public ToolProfile()
        {
            CreateMap<Tool, ToolDto>()
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name));

            CreateMap<CreateToolDto, Tool>();
            CreateMap<UpdateToolDto, Tool>();
        }
    }
}