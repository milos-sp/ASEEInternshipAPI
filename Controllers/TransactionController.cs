using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Commands;
using ProductAPI.Models;
using ProductAPI.Objects;
using ProductAPI.Services;
using ProductAPI.Validators;

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

        /*[HttpPost]
        public async Task<IActionResult> InsertTransactionAsync([FromBody] CreateTransactionCommand createTransactionCommand)
        {
            var transaction = await _transactionService.CreateTransaction(createTransactionCommand);
            if(transaction == null)
            {
                return BadRequest("Transaction already exists. Id: " + createTransactionCommand.Id);
            }

            return Ok(transaction);
        }*/

        [HttpPost("import")]
        public async Task<IActionResult> InsertTransactionsAsync([FromForm] IFormFileCollection csvFile)
        {
            // bitno je staviti u postman key csvFile
            var transactions = _csvService.ReadCSV<CreateTransactionCommand>(csvFile[0].OpenReadStream());

            var inserted = await _transactionService.InsertTransactions(transactions);

            return Ok(inserted ? "OK - Transactions inserted" : "Not inserted");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions([FromQuery] QueryObject queryObject)
        {
            var resp = new GetTransactionsValidator(queryObject.TransactionKind, queryObject.StartDate, queryObject.EndDate).ValidateParams();

            if (resp != null)
            {
                return BadRequest(resp);
            }            

            var transactions = await _transactionService.GetTransactions(queryObject);

            return Ok(transactions);
        }

        [HttpPost("{id}/categorize")]
        public async Task<IActionResult> CategorizeTransaction([FromRoute] string id, [FromBody] CategoryCode catCode)
        {

            return Ok("Transaction categorized");
        }
    }
}
