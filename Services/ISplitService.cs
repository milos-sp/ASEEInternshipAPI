using ProductAPI.Commands;

namespace ProductAPI.Services
{
    public interface ISplitService
    {
        Task<bool> SplitTransaction(string id, List<SplitTransactionCommand> splits);

        Task<bool> IsSplittedTransaction(string id);

        Task<bool> DeleteSplitsForTransaction(string id);

    }
}
