using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Repositories.Interfaces;
using ProductService.Services.Entities;

namespace ProductService.Infrastructure.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                         .Include(c => c.SubCategories)
                         .ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.Include(c => c.SubCategories)
                .FirstOrDefaultAsync(x => x.Id == id);
            ;
        }

        public async Task<IEnumerable<Category>> GetSubcategoriesAsync(int parentId)
        {
            return await _context.Categories.Where(x => x.ParentId == parentId).ToListAsync();
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
