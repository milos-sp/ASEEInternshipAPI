using ProductAPI.Commands;
using ProductAPI.Models;
using ProductAPI.Objects;

namespace ProductAPI.Services
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransaction(CreateTransactionCommand createTransactionCommand);
        Task<bool> InsertTransactions(IEnumerable<CreateTransactionCommand> transactions);
        Task<bool> DeleteTransaction(string id);
        Task<Transaction> GetTransaction(string id);
        Task<PagedSortedList<Transaction>> GetTransactions(QueryObject queryObject);

        Task<Transaction> UpdateCategory(string id, string catcode);

        Task<List<AnalyticsObject>> GetSpendingAnalytics(AnalyticsQueryObject queryObject);

        Task<bool> AutoCategorizeTransactions(List<Rule> rules);
    }
}
