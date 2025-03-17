using OrderService.Application.Dtos;

namespace OrderService.Application.Services
{
    public interface IOrderServiceLayer
    {
        Task<OrderDto> CreateOrderAsync(int productId, int quantity, decimal unitPrice);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task CompleteOrderAsync(int orderId);
    }
}
