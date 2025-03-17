using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task AddAsync(Order order);
    }
}
