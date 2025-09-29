using Microsoft.EntityFrameworkCore;
using TooliRent.Core.Interfaces;
using TooliRent.Core.Models;
using TooliRent.Infrastructure.Data;

namespace TooliRent.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TooliRentDbContext _ctx;
        public CategoryRepository(TooliRentDbContext ctx) => _ctx = ctx;

        public Task<bool> ExistsAsync(int id) =>
            _ctx.Categories.AsNoTracking().AnyAsync(c => c.Id == id);

        public Task<Category?> GetByIdAsync(int id) =>
            _ctx.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

        public async Task<IEnumerable<Category>> GetAllAsync() =>
            await _ctx.Categories.AsNoTracking().ToListAsync();
    }
}