using Microsoft.EntityFrameworkCore;
using TooliRent.Core.Interfaces;
using TooliRent.Core.Models;
using TooliRent.Core.Models.Enums;
using TooliRent.Infrastructure.Data;

namespace TooliRent.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly TooliRentDbContext _ctx;
        public BookingRepository(TooliRentDbContext ctx) => _ctx = ctx;

        public Task<Booking?> GetByIdAsync(int id) =>
            _ctx.Bookings
                .Include(b => b.Tool)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);

        public async Task<IEnumerable<Booking>> GetByUserAsync(int userId, BookingStatus? status = null)
        {
            var q = _ctx.Bookings
                .Include(b => b.Tool)
                .AsNoTracking()
                .Where(b => b.UserId == userId);

            if (status.HasValue) q = q.Where(b => b.Status == status.Value);

            return await q
                .OrderByDescending(b => b.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetRangeByIdsAsync(IEnumerable<int> ids)
        {
            var set = ids.ToHashSet();
            return await _ctx.Bookings
                .Include(b => b.Tool)
                .Where(b => set.Contains(b.Id))
                .ToListAsync();
        }

        public async Task AddAsync(Booking booking)
        {
            await _ctx.Bookings.AddAsync(booking);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Booking booking)
        {
            _ctx.Bookings.Update(booking);
            await _ctx.SaveChangesAsync();
        }

        public Task<bool> HasBlockingOverlapAsync(int toolId, DateTime start, DateTime end, int? excludeBookingId = null)
        {
            return _ctx.Bookings.AnyAsync(b =>
                b.ToolId == toolId &&
                (b.Status == BookingStatus.Approved || b.Status == BookingStatus.PickedUp) &&
                (excludeBookingId == null || b.Id != excludeBookingId.Value) &&
                start < b.EndDate && end > b.StartDate);
        }

        public async Task<int> MarkOverdueAsync(DateTime utcNow)
        {
            var affected = await _ctx.Bookings
                .Where(b => b.EndDate < utcNow && (b.Status == BookingStatus.Approved || b.Status == BookingStatus.PickedUp))
                .ExecuteUpdateAsync(s => s.SetProperty(b => b.Status, BookingStatus.Overdue));

            return affected;
        }
    }
}