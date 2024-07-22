using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Commands;
using ProductAPI.Models;
using ProductAPI.Services;

namespace ProductAPI.Controllers
{
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] SortOrder sortOrder = SortOrder.Asc, [FromQuery] string? sortBy = null)
        {
            var products = await _productService.GetProducts(page, pageSize, sortOrder, sortBy);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductCommand createProductCommand)
        {
            var product = await _productService.CreateProduct(createProductCommand);
            if (product == null) 
            {
                return BadRequest("Product already exists!");
            }
            return Ok();
        }

        // v1/products/{productCode}
        [HttpGet("{productCode}")]
        public async Task<IActionResult> GetProductAsync([FromRoute] string productCode)
        {
            var product = await _productService.GetProduct(productCode);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpDelete("{productCode}")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] string productCode)
        {
            var isDeleted = await _productService.DeleteProduct(productCode);
            if (isDeleted == false)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}