using TooliRent.Core.Models;

namespace TooliRent.Services.Services
{
    public interface IToolService
    {
        Task<IEnumerable<Tool>> GetAsync(string? category, bool? active);
        Task<Tool?> GetByIdAsync(int id);
        Task<int> CreateAsync(Tool tool);
        Task UpdateAsync(int id, Tool updated);
        Task DeleteAsync(int id);
    }
}