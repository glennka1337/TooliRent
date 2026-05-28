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

        public async Task AddAsync(Category category)
        {
            await _ctx.Categories.AddAsync(category);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            _ctx.Categories.Update(category);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _ctx.Categories.FindAsync(id);
            if (entity is null) return;
            _ctx.Categories.Remove(entity);
            await _ctx.SaveChangesAsync();
        }
    }
}