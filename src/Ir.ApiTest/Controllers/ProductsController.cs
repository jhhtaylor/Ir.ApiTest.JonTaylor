using Ir.IntegrationTest.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Ir.FakeMarketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
  private readonly ProductsService _productsService;

  public ProductsController(ProductsService productsService)
  {
    _productsService = productsService;
  }

  [HttpGet]
  public async Task<IActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
  {
    var products = await _productsService.GetAllProductsAsync(page, pageSize);
    return Ok(products);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetProduct([FromRoute] string id)
  {
    var product = await _productsService.GetProductAsync(id);
    if (product == null) return NotFound("Product not found.");

    return Ok(product);
  }

  [HttpPost]
  public async Task<IActionResult> CreateProduct([FromBody] Product product)
  {
    return await _productsService.CreateProduct(product);
  }

  [HttpPatch("{id}")]
  public async Task<IActionResult> UpdateProduct([FromRoute] string id, [FromBody] JsonPatchDocument<Product> productPatchDocument)
  {
    return await _productsService.UpdateProduct(id, productPatchDocument);
  }
}