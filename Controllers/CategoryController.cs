using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Commands;
using ProductAPI.Services;

namespace ProductAPI.Controllers
{
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICSVService _csvService;
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICSVService csvService, ICategoryService categoryService)
        {
            _logger = logger;
            _csvService = csvService;
            _categoryService = categoryService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> InsertCategoriesAsync([FromForm] IFormFileCollection csvFile)
        {
            // bitno je staviti u postman key csvFile
            var categories = _csvService.ReadCSV<CreateCategoryCommand>(csvFile[0].OpenReadStream());

            var inserted = await _categoryService.InsertCategories(categories);

            // return Ok(categories);

           return Ok(inserted ? "OK - Categories inserted" : "Not inserted");
        }

        [HttpGet]

        public async Task<IActionResult> GetCategories([FromQuery(Name = "parent-code")] string? ParentCode)
        {
            var categories = await _categoryService.GetAllCategories(ParentCode);

            return Ok(categories);
        }

    }
}
