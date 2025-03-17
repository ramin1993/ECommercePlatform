using AutoMapper;
using EventBusService.Messaging;
using Microsoft.Extensions.Logging;
using Moq;
using ProductService.Infrastructure.Repositories.Interfaces;
using ProductService.Infrastructure.UnitOfWork;
using ProductService.Services.Dtos;
using ProductService.Services.Entities;
using ProductService.Services.Implementation;
using Xunit;

namespace ProductServiceTest
{
    public class ProductServiceTest
    {
        [Fact]
        public async Task GetProductByIdAsync_ValidId_ReturnsProduct()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockRabbitMq = new Mock<IRabbitMqService>();
            mockProductRepo.Setup(repo => repo.GetByIdAsync(1))
                           .ReturnsAsync(new Product { Id = 1, Name = "Test Product", Price = 10.99m });
            mockUnitOfWork.Setup(uow => uow.Products).Returns(mockProductRepo.Object);
        
            var mockMapper = new Mock<IMapper>();          
            var mockLogger = new Mock<ILogger<ProductServiceLayer>>();
            var service = new ProductServiceLayer(mockUnitOfWork.Object, mockMapper.Object, mockLogger.Object, mockRabbitMq.Object);

            // Act
            var result = await service.GetProductByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Product", result.Name);
            Assert.Equal(10.99m, result.Price);


            //Checking Logs if need
            mockLogger.Verify(logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once());
        }
        [Fact]
        public async Task GetProductByIdAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<ProductServiceLayer>>();
            var mockRabbitMq = new Mock<IRabbitMqService>();

            var service = new ProductServiceLayer(mockUnitOfWork.Object, mockMapper.Object, mockLogger.Object, mockRabbitMq.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetProductByIdAsync(0));
        }
        [Fact]
        public async Task AddProductAsync_ValidProduct_ReturnsAddedProduct()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockProductRepo = new Mock<IProductRepository>();
            var newProduct = new Product { Id = 2, Name = "New Product", Price = 20.00m };
            var newProductDto = new ProductCreateDto { Name = "New Product", Price = 20.00m };
            mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>())).Returns(new ProductDto { Id = Guid.NewGuid(), Name = "New Product", Price = 20.00m });
            mockUnitOfWork.Setup(uow => uow.Products).Returns(mockProductRepo.Object);
            mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

            var mockLogger = new Mock<ILogger<ProductServiceLayer>>();
            var mockRabbitMq = new Mock<IRabbitMqService>();
            var service = new ProductServiceLayer(mockUnitOfWork.Object, mockMapper.Object,
                                                  mockLogger.Object, mockRabbitMq.Object);

            // Act
            var result = await service.CreateProductAsync(newProductDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Product", result.Name);
            Assert.Equal(20.00m, result.Price);

            //Checking SaveChangesAsync
            mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once());
        }

        [Fact]
        public async Task UpdateProduct_ValidProductId_UpdatesProductSuccessfully()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var mockProductRepo = new Mock<IProductRepository>();
            mockProductRepo.Setup(repo => repo.GetByIdAsync(1))
                           .ReturnsAsync(new Product { Id = 1, Name = "Old Product", Price = 10.99m });
            mockUnitOfWork.Setup(uow => uow.Products).Returns(mockProductRepo.Object);

            var mockRabbitMq = new Mock<IRabbitMqService>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<ProductServiceLayer>>();

            var service = new ProductServiceLayer(mockUnitOfWork.Object, mockMapper.Object, 
                                                   mockLogger.Object, mockRabbitMq.Object);

            var updatedProductDto = new UpdateProductDto { Name = "Updated Product", Price = 20.99m };

            // Act
            await service.UpdateProductAsync(1, updatedProductDto);

            // Assert
            mockProductRepo.Verify(repo => repo.UpdateAsync(It.Is<Product>(p => p.Id == 1 && p.Name == "Updated Product" && p.Price == 20.99m)), Times.Once);
            mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);

            // Could check to calls log
            mockLogger.Verify(logger => logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once());
        }

    }


}