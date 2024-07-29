using ProductAPI.Commands;
using ProductAPI.Database.Entities;

namespace ProductAPI.Database.Repositories
{
    public interface ISplitRepository
    {
        public Task<bool> SplitTransaction(string id, List<SplitEntity> splits);

        Task<bool> IsSplittedTransaction(string id);

        Task<bool> DeleteSplitsForTransaction(string id);

    }
}
