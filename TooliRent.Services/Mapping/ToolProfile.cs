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
                .ForCtorParam("Category", o => o.MapFrom(s => s.Category.Name));

            CreateMap<CreateToolDto, Tool>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Category, o => o.Ignore())
                .ForMember(d => d.Bookings, o => o.Ignore());

            CreateMap<UpdateToolDto, Tool>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Category, o => o.Ignore())
                .ForMember(d => d.Bookings, o => o.Ignore());
        }
    }
}