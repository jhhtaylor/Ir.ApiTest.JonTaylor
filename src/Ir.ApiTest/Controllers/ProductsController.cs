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
  public async Task<IActionResult> GetProducts()
  {
    var products = await _productsService.GetAllProductsAsync();
    return Ok(products);
  }

  [HttpGet("{id}")]
  public IActionResult GetProduct([FromRoute] string id)
  {
    throw new NotImplementedException();
  }

  [HttpPost]
  public async Task<IActionResult> CreateProduct([FromBody] Product product)
  {
    return await _productsService.CreateProduct(product);
  }

  [HttpPatch("{id}")]
  public IActionResult UpdateProduct([FromBody] JsonPatchDocument<Product> productPatchDocument)
  {
    throw new NotImplementedException();
  }
}