using ProductService.Services.Dtos;

namespace ProductService.Services.Infrastructure
{
    public interface IProductServiceLayer
    {
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
        Task<ProductResponseDto> GetProductByIdAsync(int id);
        Task<ProductResponseDto> CreateProductAsync(ProductCreateDto productDto);
        Task<bool> UpdateProductAsync(int id, UpdateProductDto productDto);
        Task<bool> DeleteProductAsync(int id);
    }
}
