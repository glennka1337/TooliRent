using TooliRent.Services.DTOs;
using TooliRent.Services.Services;
using TooliRent.Core.Interfaces;
using CoreBookingStatus = TooliRent.Core.Models.Enums.BookingStatus;

namespace TooliRent.Services.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IBookingRepository _bookings;
        public StatisticsService(IBookingRepository bookings) => _bookings = bookings;

        public async Task<StatisticsDto> GetAsync()
        {
            var total = await _bookings.CountAllAsync();
            var active = await _bookings.CountByStatusAsync(CoreBookingStatus.Approved);
            active += await _bookings.CountByStatusAsync(CoreBookingStatus.PickedUp);
            var overdue = await _bookings.CountByStatusAsync(CoreBookingStatus.Overdue);

            var topRaw = await _bookings.GetTopToolUsageAsync(10);
            var top = topRaw.Select(t => new ToolUsageDto(t.ToolName, t.UsageCount));

            return new StatisticsDto(total, active, overdue, top);
        }
    }
}
