using ProductAPI.Database.Entities;

namespace ProductAPI.Database.Repositories
{
    public interface ITransactionRepository
    {
        // Task<List<TransactionEntity>> InsertTransactions();

        Task<TransactionEntity> InsertTransactionAsync(TransactionEntity transaction);

        Task<List<TransactionEntity>> GetTransactionsAsync();

        Task DeleteTransactionAsync(TransactionEntity transaction);

        Task<TransactionEntity> GetTransactionByIdAsync(TransactionEntity transactionId);
    }
}
