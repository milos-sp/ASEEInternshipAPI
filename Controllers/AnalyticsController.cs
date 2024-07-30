using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Objects;
using ProductAPI.Services;
using ProductAPI.Validators;

namespace ProductAPI.Controllers
{
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {

        private readonly ITransactionService _transactionService;
        private readonly ILogger<AnalyticsController> _logger;
        private readonly ICategoryService _categoryService;

        public AnalyticsController(ITransactionService transactionService, ILogger<AnalyticsController> logger, ICategoryService categoryService)
        {
            _transactionService = transactionService;
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpGet("spending-analytics")]
        public async Task<IActionResult> GetSpendingAnalytics([FromQuery] AnalyticsQueryObject queryObject)
        {
            if (!String.IsNullOrEmpty(queryObject.Catcode))
            {
                if(await _categoryService.GetCategoryByCode(queryObject.Catcode) == null)
                {
                    var err = new BusinessProblemResponse();
                    err.Problem = "provided-category-does-not-exist";
                    err.Message = "Provided category does not exist";
                    err.Details = "Category with provided category code does not exist";
                    return StatusCode(440, err);
                }
                if(! await _categoryService.IsTopLevelCategory(queryObject.Catcode))
                {
                    var err = new BusinessProblemResponse();
                    err.Problem = "not-top-level-category";
                    err.Message = "Not top level category";
                    err.Details = $"CategoryCode {queryObject.Catcode} is not top level category";
                    return StatusCode(440, err);
                }
            }
            var analytics = await _transactionService.GetSpendingAnalytics(queryObject);
            return Ok(new { groups = analytics });
        }
    }
}
