using Microsoft.EntityFrameworkCore;
using ProductAPI.Database.Entities;

namespace ProductAPI.Database.Repositories
{
    public class SplitRepository : ISplitRepository
    {
        DatabaseContext _dbContext;

        public SplitRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> DeleteSplitsForTransaction(string id)
        {
            var query = _dbContext.Splits.Where(x => x.TransactionId.Equals(id));
            var count = await query.ExecuteDeleteAsync();
            Console.WriteLine("Number of deleted rows: " + count);

            return count > 0;
        }

        public async Task<bool> IsSplittedTransaction(string id)
        {
            var split = await _dbContext.Splits.FirstOrDefaultAsync(x => x.TransactionId.Equals(id));

            return split != null;
        }

        public async Task<bool> SplitTransaction(string id, List<SplitEntity> splits)
        {
            foreach (SplitEntity split in splits)
            {
                split.TransactionId = id;
                await _dbContext.Splits.AddAsync(split);
                await _dbContext.SaveChangesAsync();
            }

            return true;
        }
    }
}
