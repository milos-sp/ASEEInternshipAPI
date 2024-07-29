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
