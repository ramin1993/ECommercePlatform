using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

public class GatewayController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GatewayController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("getproductsandusers")]
    public async Task<IActionResult> GetProductsAndUsers()
    {
        // 1. Get info from UserService
        var userServiceClient = _httpClientFactory.CreateClient("UserService");
        var userResponse = await userServiceClient.GetStringAsync("/api/users/getall");

        // 2. Get info from  ProductService
        var productServiceClient = _httpClientFactory.CreateClient("ProductService");
        var productResponse = await productServiceClient.GetStringAsync("/api/products/getall");

        // 3. Returns all info
        return Ok(new
        {
            Users = userResponse,
            Products = productResponse
        });
    }
}
