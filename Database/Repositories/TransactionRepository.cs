using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using ProductAPI.Database.Entities;
using ProductAPI.Models;
using ProductAPI.Objects;
using System;
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
                if (count == 100)
                {
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

        public async Task<TransactionEntity?> GetTransactionByIdAsync(string transactionId)
        {
            return await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id.Equals(transactionId));
        }

        public async Task<TransactionEntity> InsertTransactionAsync(TransactionEntity transaction)
        {
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();

            return transaction;
        }

        public async Task<PagedSortedList<TransactionEntity>> GetTransactionsAsync(QueryObject queryObject)
        {
            var query = _dbContext.Transactions.AsQueryable();

            if (!String.IsNullOrEmpty(queryObject.TransactionKind))
            {
                query = query.Where(s => s.Kind.Equals(Enum.Parse(typeof(TransactionKind), queryObject.TransactionKind)));
            }

            if(queryObject.StartDate != null)
            {
                query = query.Where(s => s.Date >= queryObject.StartDate);
            }

            if (queryObject.EndDate != null)
            {
                query = query.Where(s => s.Date <= queryObject.EndDate);
            }

            if (!String.IsNullOrEmpty(queryObject.SortBy))
            {
                List<string> filters = queryObject.SortBy.Split(',').ToList();
                
                switch (filters.ElementAt(0)) // prvo sortira po polju koje postoji da bi se dobio ordered queryable
                {
                    case "date":
                        query = queryObject.SortOrder == SortOrder.Asc ? query.OrderBy(x => x.Date) : query.OrderByDescending(x => x.Date);
                        break;
                    case "beneficiary-name":
                        query = queryObject.SortOrder == SortOrder.Asc ? query.OrderBy(x => x.BeneficiaryName) : query.OrderByDescending(x => x.BeneficiaryName);
                        break;
                    case "description":
                        query = queryObject.SortOrder == SortOrder.Asc ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
                        break;
                    case "amount":
                        query = queryObject.SortOrder == SortOrder.Asc ? query.OrderBy(x => x.Amount) : query.OrderByDescending(x => x.Amount);
                        break;
                }
                filters.RemoveAt(0); // vec sortirano
                
                foreach (string filter in filters) {
                    var orderedQuery = query as IOrderedQueryable<TransactionEntity>;
                    switch (filter)
                    {
                        case "date":
                            query = queryObject.SortOrder == SortOrder.Asc ? orderedQuery.ThenBy(x => x.Date) : orderedQuery.ThenByDescending(x => x.Date);
                            break;
                        case "beneficiary-name":
                            query = queryObject.SortOrder == SortOrder.Asc ? orderedQuery.ThenBy(x => x.BeneficiaryName) : orderedQuery.ThenByDescending(x => x.BeneficiaryName);
                            break;
                        case "description":
                            query = queryObject.SortOrder == SortOrder.Asc ? orderedQuery.ThenBy(x => x.Description) : orderedQuery.ThenByDescending(x => x.Description);
                            break;
                        case "amount":
                            query = queryObject.SortOrder == SortOrder.Asc ? orderedQuery.ThenBy(x => x.Amount) : orderedQuery.ThenByDescending(x => x.Amount);
                            break;
                    }
                }
            }
            else
            {
                query = query.OrderBy(x => x.BeneficiaryName);
            }

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount * 1.0 / queryObject.PageSize);

            query = query.Skip((queryObject.Page - 1) * queryObject.PageSize).Take(queryObject.PageSize);

            var transactions = await query.ToListAsync();

            return new PagedSortedList<TransactionEntity>
            {
                TotalPages = totalPages,
                TotalCount = totalCount,
                Page = queryObject.Page,
                PageSize = queryObject.PageSize,
                SortBy = queryObject.SortBy,
                SortOrder = queryObject.SortOrder,
                Items = transactions
            };
        }

        public async Task<TransactionEntity?> UpdateCategoryAsync(string transactionId, string catcode)
        {
            var existingTransaction = await _dbContext.Transactions.FirstOrDefaultAsync(x => x.Id.Equals(transactionId));

            if (existingTransaction != null)
            {
                existingTransaction.Catcode = catcode;
                await _dbContext.SaveChangesAsync();
                return existingTransaction;
            } else
            {
                return null;
            }
        }
    }
}
