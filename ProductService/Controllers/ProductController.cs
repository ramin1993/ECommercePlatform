using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Services.Dtos;
using ProductService.Services.Infrastructure;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductServiceLayer _productServiceLayer;

    public ProductController(IProductServiceLayer productServiceLayer)
    {
        _productServiceLayer = productServiceLayer;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAllProducts()
    {
        var products = await _productServiceLayer.GetAllProductsAsync();
        return Ok(products);
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetProductById(int id)
    {
        var product = await _productServiceLayer.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    [Authorize(Roles = "Admin")] //Only Admin
    public async Task<ActionResult<ProductResponseDto>> CreateProduct([FromBody] ProductCreateDto productDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdProduct = await _productServiceLayer.CreateProductAsync(productDto);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    // PUT: api/products/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] //Only Admin
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto productDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _productServiceLayer.UpdateProductAsync(id, productDto);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] //Only Admin
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await _productServiceLayer.DeleteProductAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}

