using Microsoft.EntityFrameworkCore;
using TooliRent.Infrastructure.Data;
using TooliRent.Services.DTOs;

namespace TooliRent.Services.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly TooliRentDbContext _ctx;
        public StatisticsService(TooliRentDbContext ctx) => _ctx = ctx;

        public async Task<StatisticsDto> GetAsync()
        {
            var total = await _ctx.Bookings.CountAsync();
            var active = await _ctx.Bookings.CountAsync(b => b.Status == Core.Models.Enums.BookingStatus.Approved || b.Status == Core.Models.Enums.BookingStatus.PickedUp);
            var overdue = await _ctx.Bookings.CountAsync(b => b.Status == Core.Models.Enums.BookingStatus.Overdue);

            var top = await _ctx.Bookings
                .Include(b => b.Tool)
                .GroupBy(b => b.Tool.Name)
                .Select(g => new ToolUsageDto(g.Key, g.Count()))
                .OrderByDescending(t => t.UsageCount)
                .Take(10)
                .ToListAsync();

            return new StatisticsDto(total, active, overdue, top);
        }
    }
}
