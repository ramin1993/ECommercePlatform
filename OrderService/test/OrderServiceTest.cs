using AutoMapper;
using Moq;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Repositories;
using Xunit;
using OrderService.Application.Services;
using EventBusService.Messaging;

namespace ServiceOrder;

public class OrderServiceTest
{
    [Fact]
    public async Task CompleteOrderAsync_ValidId_CompletesSuccessfully()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var order = new Order(1, 2, 10m);
        var mockUnit = new Mock<IUnitOfWork>();
        var mockRabbitMq = new Mock<IRabbitMqService>();
        mockUnit.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        var mockMapper = new Mock<IMapper>();
        var mockLogger = new Mock<ILogger<OrderServiceLayer>>();

        var service = new OrderServiceLayer(mockRepo.Object, mockUnit.Object, mockMapper.Object, mockLogger.Object, mockRabbitMq.Object);

        // Act
        await service.CompleteOrderAsync(1);

        // Assert
        Assert.Equal("Completed", order.Status);
        mockUnit.Verify(u => u.SaveChangesAsync(), Times.Once());
    }
}
