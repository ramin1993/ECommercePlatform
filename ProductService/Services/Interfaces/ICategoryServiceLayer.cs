using ProductService.Services.Dtos;

namespace ProductService.Services.Infrastructure
{
    public interface ICategoryServiceLayer
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetSubcategoriesAsync(int parentId);
        Task<CategoryDto> AddCategoryAsync(CreateCategoryDto dto);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
