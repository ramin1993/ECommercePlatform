using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Services.Dtos;
using ProductService.Services.Infrastructure;

namespace ProductService.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServiceLayer _categoryService;

        public CategoryController(ICategoryServiceLayer categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] //Only Admin
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var createdCategory = await _categoryService.AddCategoryAsync(dto);
            return CreatedAtAction(nameof(GetAllCategories), new { id = createdCategory.Id }, createdCategory);
        }
    }

}
