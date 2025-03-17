using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductService.Services.Entities;

namespace ProductService.Infrastructure.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id); // Primary Key

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200); // Maks 200 simb

            builder.Property(p => p.Description)
                   .HasMaxLength(500); // Maks 500 simb (optional)

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)") // 18-digit, 2-digit decimal
                   .IsRequired();

            builder.Property(p => p.StockQuantity)
                   .IsRequired();

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()") // Default UTC time
                   .ValueGeneratedOnAdd();

            // Product - Category əlaqəsi
            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade); // When a Category delete products will delete

            builder.ToTable("Products"); // Table name
        }
    }
    }
