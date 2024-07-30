using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        // za proveru postoji li kategorija
        private readonly ICategoryService _categoryService;
        // za split transakcije
        private readonly ISplitService _splitService;
        public TransactionController(ILogger<TransactionController> logger, ITransactionService transactionService, ICSVService csvService, ICategoryService categoryService, ISplitService splitService)
        {
            _logger = logger;
            _transactionService = transactionService;
            _csvService = csvService;
            _categoryService = categoryService;
            _splitService = splitService;
        }

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
                return BadRequest(new { errors = resp } );
            }            

            var transactions = await _transactionService.GetTransactions(queryObject);

            return Ok(transactions);
        }

        [HttpPost("{id}/categorize")]
        public async Task<IActionResult> CategorizeTransaction([FromRoute] string id, [FromBody] CategoryCode catcode)
        {
            var validator = new CategorizeTransactionValidator(id, catcode, _transactionService, _categoryService);
            var resp = validator.ValidateParams();

            if (resp != null)
            {
                return BadRequest(new { errors = resp} );
            }

            // ako su parametri ok, treba proveriti da li postoje transakcija i kategorija
            var err = await validator.CheckIfExists(id, catcode.Catcode);

            if (err != null)
            {
                return StatusCode(440, err);
            }

            var transaction = await _transactionService.UpdateCategory(id, catcode.Catcode);

            return Ok("Transaction categorized");
        }

        [HttpPost("{id}/split")]
        public async Task<IActionResult> SplitTransaction([FromRoute] string id, [FromBody] List<SplitTransactionCommand> splits)
        {
            // kao stock i comments
            // imace listu transakcija koje su nastale od neke
            // u novoj tabeli je id transakcije strani kljuc za sve splitovane
            // pored stranog kljuca moze autogenerisani id i amount
            // ako vec postoje neke sa id kao stranim kljucem brisu se pa se splituje ponovo po zahtevu
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = await _transactionService.GetTransaction(id);

            if(transaction == null)
            {
                var err = new BusinessProblemResponse();
                err.Problem = "transaction-does-not-exist";
                err.Message = "Transaction does not exist";
                err.Details = "Transaction with provided id does not exist";

                return StatusCode(440, err);
            }

            var sum = 0.0;
            foreach (var split in splits)
            {
                sum += split.Amount;
            }

            if (sum != transaction.Amount)
            {
                var err = new BusinessProblemResponse();
                err.Problem = "split-amount-not-same-as-transaction-amount";
                err.Message = "Split amount not same as transaction amount";
                err.Details = "Sum of amounts from splits must be equal to transaction amount";

                return StatusCode(440, err);
            }

            // ako je transakcija vec splitovana, obrisati splitove pa ponovo splitovati

            if (await _splitService.IsSplittedTransaction(id))
            {
                await _splitService.DeleteSplitsForTransaction(id);
            }

            await _splitService.SplitTransaction(id, splits);

            return Ok("OK - Transaction splitted");
        }
    }
}
