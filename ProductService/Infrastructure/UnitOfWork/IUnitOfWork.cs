using ProductService.Infrastructure.Repositories.Interfaces;

namespace ProductService.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        Task<int> CompleteAsync();
    }
}
