using AutoMapper;
using EventBusService.Messaging;
using ProductService.Infrastructure.UnitOfWork;
using ProductService.Services.Dtos;
using ProductService.Services.Entities;
using ProductService.Services.Exceptions;
using ProductService.Services.Infrastructure;
using Share.Messages;

namespace ProductService.Services.Implementation
{
    public class ProductServiceLayer : IProductServiceLayer
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductServiceLayer> _logger;
        private readonly IRabbitMqService _rabbitMqService;

        public ProductServiceLayer(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<ProductServiceLayer> logger, IRabbitMqService rabbitMqService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _rabbitMqService = rabbitMqService;
            _rabbitMqService.Subscribe<ProductPriceRequest>(
                queue: "product-price-queue",
                exchange: "product-exchange",
                routingKey: "price.request", handler: async (request) =>
                {
                    var product = await GetProductByIdAsync(request.ProductId);
                    var response = new ProductPriceResponse
                    {
                        ProductId = request.ProductId,
                        Price = product.Price,
                        IsAvailable = true // Stok yoxlaması əlavə edilə bilər
                    };
                    _rabbitMqService.Publish(response, "product-exchange", "price.response");
                });
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("GetProductAsync method called with productId: {id}", id);

                var product = await _unitOfWork.Products.GetByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product with Id {ProductId} not found",id);
                    throw new NotFoundException("Product not found");
                }
                return  product != null ? _mapper.Map<ProductResponseDto>(product) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching product with Id {id}", id);
                throw;
            }



        }

        public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductDto productDto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return false;
            }

            _mapper.Map(productDto, product);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            await _unitOfWork.Products.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }

    }
