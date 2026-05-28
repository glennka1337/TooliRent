using Microsoft.EntityFrameworkCore;
using TooliRent.Core.Interfaces;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TooliRent.Infrastructure.Repositories
{
    public class ToolRepository : IToolRepository
    {
        private readonly TooliRentDbContext _context;

        public ToolRepository(TooliRentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tool>> GetAllAsync(string? category = null, bool? onlyActive = null, bool? onlyAvailable = null)
        {
            IQueryable<Tool> query = _context.Tools
                .Include(t => t.Category)
            .Include(t => t.Bookings)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(t => t.Category.Name == category);

            if (onlyActive.HasValue)
                query = query.Where(t => t.IsActive == onlyActive.Value);

            if (onlyAvailable.HasValue)
            {
                var now = DateTime.UtcNow;
                if (onlyAvailable.Value)
                {
                    query = query.Where(t => !t.Bookings.Any(b => (b.Status == TooliRent.Core.Models.Enums.BookingStatus.Approved || b.Status == TooliRent.Core.Models.Enums.BookingStatus.PickedUp) && b.StartDate <= now && b.EndDate >= now));
                }
                else
                {
                    query = query.Where(t => t.Bookings.Any(b => (b.Status == TooliRent.Core.Models.Enums.BookingStatus.Approved || b.Status == TooliRent.Core.Models.Enums.BookingStatus.PickedUp) && b.StartDate <= now && b.EndDate >= now));
                }
            }

            return await query.ToListAsync();
        }

        public async Task<Tool?> GetByIdAsync(int id)
        {
            return await _context.Tools
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Tool tool)
        {
            await _context.Tools.AddAsync(tool);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tool tool)
        {
            _context.Tools.Update(tool);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Tools.FindAsync(id);
            if (entity is null) return;
            _context.Tools.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
