using AutoMapper;
using ProductService.Infrastructure.UnitOfWork;
using ProductService.Services.Dtos;
using ProductService.Services.Entities;
using ProductService.Services.Infrastructure;

namespace ProductService.Services.Implementation
{
    public class CategoryServiceLayer:ICategoryServiceLayer
    {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public CategoryServiceLayer(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
            {
                var categories = await _unitOfWork.Categories.GetAllCategoriesAsync();
                return _mapper.Map<IEnumerable<CategoryDto>>(categories);
            }

            public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
            {
                var category = await _unitOfWork.Categories.GetCategoryByIdAsync(id);
                return category != null ? _mapper.Map<CategoryDto>(category) : null;
            }

        public async Task<IEnumerable<CategoryDto>> GetSubcategoriesAsync(int parentId)
        {
            var category= await _unitOfWork.Categories.GetSubcategoriesAsync(parentId);
            return (IEnumerable<CategoryDto>)_mapper.Map<CategoryDto>(category);
        }
        public async Task<CategoryDto> AddCategoryAsync(CreateCategoryDto dto)
            {
                var category = _mapper.Map<Category>(dto);
                await _unitOfWork.Categories.AddCategoryAsync(category);
                await _unitOfWork.CompleteAsync(); // Unit of Work vasitəsilə təsdiqləyirik
                return _mapper.Map<CategoryDto>(category);
            }

            public async Task<bool> DeleteCategoryAsync(int id)
            {
                bool deleted = await _unitOfWork.Categories.DeleteCategoryAsync(id);
                if (deleted)
                {
                    await _unitOfWork.CompleteAsync();
                }
                return deleted;
            }
        }

    }
