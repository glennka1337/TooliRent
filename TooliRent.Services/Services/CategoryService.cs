using TooliRent.Core.Interfaces;
using TooliRent.Core.Models;

namespace TooliRent.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo) => _repo = repo;

        public Task<IEnumerable<Category>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Category?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<int> CreateAsync(Category category)
        {
            await _repo.AddAsync(category);
            return category.Id;
        }

        public async Task UpdateAsync(int id, Category updated)
        {
            var existing = await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Category not found.");
            existing.Name = updated.Name;
            await _repo.UpdateAsync(existing);
        }

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
