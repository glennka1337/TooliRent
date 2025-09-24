using TooliRent.Core.Models;

namespace TooliRent.Core.Interfaces
{
    public interface IToolRepository
    {
        Task<IEnumerable<Tool>> GetAllAsync(string? category = null, bool? onlyActive = null);
        Task<Tool?> GetByIdAsync(int id);
        Task AddAsync(Tool tool);
        Task UpdateAsync(Tool tool);
        Task DeleteAsync(int id);
    }
}