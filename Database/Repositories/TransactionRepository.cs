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

        public async Task<int> InsertBulkTransactions(List<TransactionEntity> transactions)
        {
            int count = 0;
            int countTotal = 0;
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false; // za bolje performanse
            foreach (TransactionEntity transaction in transactions) // podela po 100 upisa
            {
                _dbContext.Transactions.Add(transaction);
                count++;
                if (count == 100) {
                    count = 0;
                    countTotal += 100;
                    // await _dbContext.SaveChangesAsync();
                    _dbContext.SaveChanges();
                }
            }
            // await _dbContext.SaveChangesAsync();
            _dbContext.SaveChanges();
            countTotal += count;

            _dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
            return countTotal;
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
