using TooliRent.Core.Models;
using TooliRent.Core.Models.Enums;

namespace TooliRent.Core.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id);
        Task<IEnumerable<Booking>> GetByUserAsync(int userId, BookingStatus? status = null);
        Task<IEnumerable<Booking>> GetRangeByIdsAsync(IEnumerable<int> ids);
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task<bool> HasBlockingOverlapAsync(int toolId, DateTime start, DateTime end, int? excludeBookingId = null);
        Task<int> MarkOverdueAsync(DateTime utcNow);
        // Additional stats queries
        Task<int> CountAllAsync();
        Task<int> CountByStatusAsync(BookingStatus status);
        Task<IEnumerable<(string ToolName, int UsageCount)>> GetTopToolUsageAsync(int topN);
    }
}