using ProductService.Services.Entities;

namespace ProductService.Infrastructure.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<IEnumerable<Category>> GetSubcategoriesAsync(int parentId);
        Task<Category> AddCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
