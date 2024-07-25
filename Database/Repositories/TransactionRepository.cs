using Microsoft.EntityFrameworkCore;
using ProductAPI.Database.Entities;
using ProductAPI.Models;
using System.Globalization;

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

        public async Task<PagedSortedList<TransactionEntity>> GetTransactionsAsync(int page = 1, int pageSize = 10, SortOrder sortOrder = SortOrder.Asc, string? sortBy = null)
        {
            var query = _dbContext.Transactions.AsQueryable();
            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount * 1.0 / pageSize);

            var transactionsE = _dbContext.Transactions;

            List<TransactionEntity> lists;

            if (!String.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "date":
                        transactionsE.AsEnumerable().OrderBy(t => DateTime.ParseExact(t.Date, "mm/dd/yyyy", CultureInfo.InvariantCulture)).ToList();
                        // query = sortOrder == SortOrder.Asc ? query.OrderBy(x => DateTime.ParseExact(x.Date, "mm/dd/yyyy", CultureInfo.InvariantCulture)) : query.OrderByDescending(x => DateTime.ParseExact(x.Date, "mm/dd/yyyy", CultureInfo.InvariantCulture));
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Date) : query.OrderByDescending(x => x.Date);
                        query = transactionsE.AsQueryable();
                        break;
                    case "beneficiary-name":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.BeneficiaryName) : query.OrderByDescending(x => x.BeneficiaryName);
                        break;
                    case "description":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
                        break;
                    case "amount":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Amount) : query.OrderByDescending(x => x.Amount);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(x => x.BeneficiaryName);
            }

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var transactions = await query.ToListAsync();

            return new PagedSortedList<TransactionEntity>
            {
                TotalPages = totalPages,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                SortOrder = sortOrder,
                Items = transactions
            };
        }
    }
}
