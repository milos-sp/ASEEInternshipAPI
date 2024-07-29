using ProductAPI.Commands;

namespace ProductAPI.Services
{
    public interface ISplitService
    {
        Task<bool> SplitTransaction(string id, List<SplitTransactionCommand> splits);

    }
}
