using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProductService.Services.Entities;

namespace ProductService.Infrastructure.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id); // Primary Key

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(150); // Maks length 150 simvol

            // own  referance - ParentCategory
            builder.HasOne(c => c.ParentCategory)
                   .WithMany(c => c.SubCategories)
                   .HasForeignKey(c => c.ParentId)
                   .OnDelete(DeleteBehavior.Restrict); // Restrict:Safe Communicate while deleting

            // Category - Product relation (A category can take many product)
            builder.HasMany(c => c.Products)
                   .WithOne(p => p.Category)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade); // Deleting category product will delete together

            builder.ToTable("Categories"); // to appoint table name
        }
    }
}
