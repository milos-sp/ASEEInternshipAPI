using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Commands;
using ProductAPI.Models;
using ProductAPI.Services;

namespace ProductAPI.Controllers
{
    [EnableCors("MyCORSPolicy")]
    [ApiController]
    [Route("transactions")]
    public class TransactionController : ControllerBase // mapiranja?
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionController> _logger;
        private readonly ICSVService _csvService;
        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService, ICSVService csvService)
        {
            _logger = logger;
            _transactionService = transactionService;
            _csvService = csvService;
        }

        [HttpPost]
        public async Task<IActionResult> InsertTransactionAsync([FromBody] CreateTransactionCommand createTransactionCommand)
        {
            var transaction = await _transactionService.CreateTransaction(createTransactionCommand);
            if(transaction == null)
            {
                return BadRequest("Transaction already exists. Id: " + createTransactionCommand.Id);
            }

            return Ok(transaction);
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertTransactionsAsync([FromForm] IFormFileCollection csvFile)
        {
            // bitno je staviti u postman key csvFile
            var transactions = _csvService.ReadCSV<CreateTransactionCommand>(csvFile[0].OpenReadStream());

            var inserted = await _transactionService.InsertTransactions(transactions);

            return Ok(inserted ? "OK - Transactions inserted" : "Not inserted");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions([FromQuery] int page = 1, [FromQuery(Name = "page-size")] int pageSize = 10, [FromQuery(Name = "sort-by")] string? sortBy = null, [FromQuery(Name = "sort-order")] SortOrder sortOrder = SortOrder.Asc)
        {
            var transactions = await _transactionService.GetTransactions(page, pageSize, sortOrder, sortBy);

            return Ok(transactions);
        }
    }
}
