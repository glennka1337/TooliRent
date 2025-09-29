using TooliRent.Core.Models;

namespace TooliRent.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<bool> ExistsAsync(int id);
        Task<Category?> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
    }
}