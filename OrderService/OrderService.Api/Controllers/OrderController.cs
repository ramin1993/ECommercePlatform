using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Services;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServiceLayer _orderService;

        public OrderController(IOrderServiceLayer orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var orderDto = await _orderService.CreateOrderAsync(request.ProductId, request.Quantity, request.UnitPrice);
            return Ok(orderDto);
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteOrder(int id)
        {
            await _orderService.CompleteOrderAsync(id);
            return NoContent();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")] // Yalnız Admin görə bilər
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
    }

    public class CreateOrderRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}