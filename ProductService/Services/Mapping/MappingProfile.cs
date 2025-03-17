using AutoMapper;
using ProductService.Services.Dtos;
using ProductService.Services.Entities;

namespace ProductService.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product,         ProductResponseDto>().ReverseMap();
            CreateMap<Product,         ProductCreateDto>().ReverseMap();
            CreateMap<Product,         UpdateProductDto>().ReverseMap();
            CreateMap<Product,         ProductDto>().ReverseMap();
            CreateMap<Category,        CategoryDto>().ReverseMap();
            CreateMap<Category,        CreateCategoryDto>().ReverseMap();

        }
    }
}
