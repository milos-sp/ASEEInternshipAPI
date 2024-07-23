using Microsoft.EntityFrameworkCore;
using ProductAPI.Database.Entities;

namespace ProductAPI.Database.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        DatabaseContext _dbContext;
        public TransactionRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task DeleteTransactionAsync(TransactionEntity transaction)
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionEntity> GetTransactionByIdAsync(string transactionId)
        {
            return await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id.Equals(transactionId));
        }

        public Task<List<TransactionEntity>> GetTransactionsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<TransactionEntity> InsertTransactionAsync(TransactionEntity transaction)
        {
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();

            return transaction;
        }
    }
}
