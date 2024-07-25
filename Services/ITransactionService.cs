using ProductAPI.Commands;
using ProductAPI.Models;

namespace ProductAPI.Services
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransaction(CreateTransactionCommand createTransactionCommand);
        Task<bool> InsertTransactions(IEnumerable<CreateTransactionCommand> transactions);
        Task<bool> DeleteTransaction(string id);
        Task<Transaction> GetTransaction(string id);
        Task<PagedSortedList<Transaction>> GetTransactions(string? transactionKind, int page, int pageSize, SortOrder sortOrder, string? sortBy);
    }
}
