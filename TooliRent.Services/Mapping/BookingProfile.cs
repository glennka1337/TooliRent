using AutoMapper;
using TooliRent.Core.Models;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Mapping
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingDto>()
                .ForCtorParam("ToolName", o => o.MapFrom(s => s.Tool.Name));
        }
    }
}