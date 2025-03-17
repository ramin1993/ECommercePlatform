namespace ProductService.Services.Dtos
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public int? ParentId { get; set; }
    }
}
