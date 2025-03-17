namespace OrderService.Infrastructure.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
