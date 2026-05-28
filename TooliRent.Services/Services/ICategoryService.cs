using TooliRent.Core.Models;

namespace TooliRent.Services.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<int> CreateAsync(Category category);
        Task UpdateAsync(int id, Category updated);
        Task DeleteAsync(int id);
    }
}
