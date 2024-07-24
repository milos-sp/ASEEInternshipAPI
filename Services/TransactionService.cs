using AutoMapper;
using ProductAPI.Commands;
using ProductAPI.Database.Entities;
using ProductAPI.Database.Repositories;
using ProductAPI.Models;

namespace ProductAPI.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;

        private readonly IMapper _mapper;

        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ITransactionRepository repository, IMapper mapper, ILogger<TransactionService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        private async Task<bool> CheckIfProductExistsAsync(string id)
        {
            var transaction = await _repository.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return false;

            }
            return true;
        }

        public async Task<Transaction> CreateTransaction(CreateTransactionCommand createTransactionCommand)
        {
            var transactionExists = await CheckIfProductExistsAsync(createTransactionCommand.Id);

            if (!transactionExists)
            {
                var newTransactionEntity = _mapper.Map<TransactionEntity>(createTransactionCommand);
                await _repository.InsertTransactionAsync(newTransactionEntity);
                return _mapper.Map<Transaction>(newTransactionEntity);
            }

            return null;
        }

        public Task<bool> DeleteTransaction(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedSortedList<Transaction>> GetTransactions(int page, int pageSize, SortOrder sortOrder, string? sortBy)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> GetTransaction(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertTransactions(IEnumerable<CreateTransactionCommand> transactions)
        {
            int index = 1;
            List<TransactionEntity> transactionsList = new List<TransactionEntity>();
            foreach (var transaction in transactions)
            {
                var transactionExists = await CheckIfProductExistsAsync(transaction.Id);
                if (!transactionExists)
                {
                    // potrebna provera da li su sva obavezna polja ispravna
                    if (transaction.Date.Equals(""))
                    {
                        _logger.LogWarning("Row " + index + ": Date field is missing");
                    }
                    else if (transaction.Amount == null)
                    {
                        _logger.LogWarning("Row " + index + ": Amount field is missing");
                    }
                    else if(transaction.Currency.Equals("")){
                        _logger.LogWarning("Row " + index + ": Currency field is missing");
                    }
                    else if (transaction.Direction == null)
                    {
                        _logger.LogWarning("Row " + index + ": Direction field is missing");
                    }
                    else if (transaction.Kind == null)
                    {
                        _logger.LogWarning("Row " + index + ": Kind field is missing");
                    }
                    else
                    {
                        var newTransactionEntity = _mapper.Map<TransactionEntity>(transaction);
                        transactionsList.Add(newTransactionEntity);
                    }
                }
                else
                {
                    _logger.LogWarning($"Insert Transaction: Row {index} - Transaction with id {transaction.Id} already exists");
                }

                index++;
            }

            // insert samo validnih redova
            int count = await _repository.InsertBulkTransactions(transactionsList);

            _logger.LogInformation($"Number of inserted rows: {count}");

            return true;
        }
    }
}
