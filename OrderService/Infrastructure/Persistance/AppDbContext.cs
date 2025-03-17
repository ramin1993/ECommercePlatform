using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Configurations;

namespace OrderService.Infrastructure.Persistance
{

        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
           public  DbSet<Order> Orders { get; set; }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.ApplyConfiguration(new OrderConfiguration());
                base.OnModelCreating(modelBuilder);
            }
        }
    
}
