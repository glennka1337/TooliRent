using TooliRent.Services.DTOs;

namespace TooliRent.Services.Services
{
    public interface IStatisticsService
    {
        Task<StatisticsDto> GetAsync();
    }
}
