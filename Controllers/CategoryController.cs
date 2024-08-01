using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Commands;
using ProductAPI.Models;
using ProductAPI.Services;
using ProductAPI.Validators;

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
            if (csvFile.Count() == 0)
            {
                var resp = new ValidatorErrorResponse();
                resp.Tag = "no-csv";
                resp.Error = "CSV not imported";
                resp.Message = $"CSV file not selected";
                return BadRequest(new { errors = resp });
            }
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

            return Ok(new { items = categories });
        }

    }
}
