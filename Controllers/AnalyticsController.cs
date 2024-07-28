using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Objects;
using ProductAPI.Services;

namespace ProductAPI.Controllers
{
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {

        private readonly ITransactionService _transactionService;
        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(ITransactionService transactionService, ILogger<AnalyticsController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        [HttpGet("spending-analytics")]
        public async Task<IActionResult> GetSpendingAnalytics([FromQuery] AnalyticsQueryObject queryObject)
        {
            var analytics = await _transactionService.GetSpendingAnalytics(queryObject);
            return Ok(new { groups = analytics });
        }
    }
}
