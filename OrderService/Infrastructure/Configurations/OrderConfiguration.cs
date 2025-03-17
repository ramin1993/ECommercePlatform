using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Configurations
{

        public class OrderConfiguration : IEntityTypeConfiguration<Order>
        {
            public void Configure(EntityTypeBuilder<Order> builder)
            {
                builder.HasKey(o => o.Id);
                builder.Property(o => o.ProductId).IsRequired();
                builder.Property(o => o.Quantity).IsRequired();
                builder.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)").IsRequired();
                builder.Property(o => o.Status).IsRequired();
                builder.Property(o => o.OrderDate).IsRequired();
            }
        }
    
}
