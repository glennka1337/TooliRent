using TooliRent.Core.Interfaces;
using TooliRent.Core.Models;

namespace TooliRent.Services.Services
{
    public class ToolService : IToolService
    {
        private readonly IToolRepository _repo;
        private readonly ICategoryRepository _categories;

        public ToolService(IToolRepository repo, ICategoryRepository categories)
        {
            _repo = repo;
            _categories = categories;
        }

        public Task<IEnumerable<Tool>> GetAsync(string? category, bool? active) =>
            _repo.GetAllAsync(category, active);

        public Task<Tool?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<int> CreateAsync(Tool tool)
        {
            if (!await _categories.ExistsAsync(tool.CategoryId))
                throw new ArgumentException("CategoryId does not exist.");
            await _repo.AddAsync(tool);
            return tool.Id;
        }

        public async Task UpdateAsync(int id, Tool updated)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing is null)
                throw new KeyNotFoundException("Tool not found");

            if (!await _categories.ExistsAsync(updated.CategoryId))
                throw new ArgumentException("CategoryId does not exist.");

            existing.Name = updated.Name;
            existing.Description = updated.Description;
            existing.CategoryId = updated.CategoryId;
            existing.IsActive = updated.IsActive;

            await _repo.UpdateAsync(existing);
        }

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}