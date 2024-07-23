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

        public TransactionService(ITransactionRepository repository, IMapper mapper)
        {
           _repository = repository;
           _mapper = mapper;
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
    }
}
