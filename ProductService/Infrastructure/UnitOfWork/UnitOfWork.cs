using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Repositories.Interfaces;

namespace ProductService.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }

        public UnitOfWork(ApplicationDbContext context,
                          IProductRepository productRepository,
                          ICategoryRepository categoryRepository)
        {
            _context = context;
            Products = productRepository;
            Categories = categoryRepository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
