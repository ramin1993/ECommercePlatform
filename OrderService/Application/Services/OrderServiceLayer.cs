using AutoMapper;
using EventBusService.Messaging;
using OrderService.Application.Dtos;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Repositories;
using OrderService.OrderService.Api.Exceptios;
using Share.Messages;

namespace OrderService.Application.Services
{
    public class OrderServiceLayer:IOrderServiceLayer
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderServiceLayer> _logger;
        private readonly IRabbitMqService _rabbitMqService;
        private ProductPriceResponse _priceResponse;


        public OrderServiceLayer(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<OrderServiceLayer> logger,
            IRabbitMqService rabbitMqService)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _rabbitMqService = rabbitMqService;

            // Subscribe to price response
            _rabbitMqService.Subscribe<ProductPriceResponse>(
                queue: "order-price-queue",
                exchange: "product-exchange",
                routingKey: "price.response",
                handler: (response) => _priceResponse = response);
        }

        public async Task<OrderDto> CreateOrderAsync(int productId, int quantity, decimal unitPrice)
        {
            _logger.LogInformation($"Creating order for Product ID: {productId}");
            //Send price request
            _rabbitMqService.Publish(new ProductPriceRequest { ProductId = productId },
                "product-exchange", "price.request");
            await Task.Delay(100);
            if (_priceResponse == null || _priceResponse.ProductId != productId)
                throw new Exception("Failed to get product price.");

            var order = new Order(productId, quantity, unitPrice); // Domaindən istifadə
            if (order == null) throw new OrderNotFoundException(order.Id);
            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OrderDto>(order);
        }

        public async Task CompleteOrderAsync(int orderId)
        {
            _logger.LogInformation($"Completing order ID: {orderId}");
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException("Order not found.");

            order.Complete(); // Domain qaydasını çağırırıq
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            _logger.LogInformation("Retrieving all orders...");
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<List<OrderDto>>(orders);
        }
    }
}
