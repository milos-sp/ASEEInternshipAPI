using ProductAPI.Database.Entities;
using ProductAPI.Models;

namespace ProductAPI.Database.Repositories
{
    public interface ITransactionRepository
    {
        Task<TransactionEntity> InsertTransactionAsync(TransactionEntity transaction);

        Task<int> InsertBulkTransactions(List<TransactionEntity> transactions);

        Task<PagedSortedList<TransactionEntity>> GetTransactionsAsync(string? transactionKind, int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc, string? sortBy = null);

        Task DeleteTransactionAsync(TransactionEntity transaction);

        Task<TransactionEntity> GetTransactionByIdAsync(string transactionId);
    }
}
