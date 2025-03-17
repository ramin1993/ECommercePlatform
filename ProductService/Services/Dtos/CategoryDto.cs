namespace ProductService.Services.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<CategoryDto>? SubCategories { get; set; }
    }
}
