using TooliRent.Core.Interfaces;
using TooliRent.Core.Models;

namespace TooliRent.Services.Services
{
    public class ToolService : IToolService
    {
        private readonly IToolRepository _repo;

        public ToolService(IToolRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Tool>> GetAsync(string? category, bool? active) =>
            _repo.GetAllAsync(category, active);

        public Task<Tool?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

        public async Task<int> CreateAsync(Tool tool)
        {
            await _repo.AddAsync(tool);
            return tool.Id;
        }

        public async Task UpdateAsync(int id, Tool updated)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing is null)
                throw new KeyNotFoundException("Tool not found");

            existing.Name = updated.Name;
            existing.Description = updated.Description;
            existing.CategoryId = updated.CategoryId;
            existing.IsActive = updated.IsActive;

            await _repo.UpdateAsync(existing);
        }

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}