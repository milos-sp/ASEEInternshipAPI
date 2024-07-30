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
            var query = _dbContext.Transactions.Include(t => t.Splits).AsQueryable();

            if (!String.IsNullOrEmpty(queryObject.TransactionKind))
            {
                List<string> filters = queryObject.TransactionKind.Split(',').ToList();
                // var predicate = PredicateBuilder
                var kinds = new List<TransactionKind>();
                foreach(var filter in filters)
                {
                    TransactionKind k = (TransactionKind) Enum.Parse(typeof(TransactionKind), filter);
                    kinds.Add(k);
                }
                query = query.Where(s => kinds.Contains(s.Kind));
                // query = query.Where(s => s.Kind.Equals(Enum.Parse(typeof(TransactionKind), queryObject.TransactionKind)));
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

        public async Task<List<AnalyticsObject>> GetSpendingAnalytics(AnalyticsQueryObject queryObject)
        {
            var query = _dbContext.Transactions.Where(x => !String.IsNullOrEmpty(x.Catcode));
            var setTop = new HashSet<string?>();
            if (!String.IsNullOrEmpty(queryObject.Catcode))
            {
                query = query.Where(x => x.Catcode.Equals(queryObject.Catcode) || x.Category.ParentCode.Equals(queryObject.Catcode)); // ako treba prikazati analitiku samo za jednu kategoriju
            }
            else
            {
                // samo top level za sve kategorije
                setTop = _dbContext.Transactions.Where(x => x.Category.ParentCode.Equals("")).Select(x => x.Catcode).ToHashSet();
            }

            if (queryObject.Direction != null)
            {
                query = query.Where(x => x.Direction.Equals(queryObject.Direction));
            }

            if(queryObject.StartDate != null)
            {
                query = query.Where(x => x.Date >=  queryObject.StartDate);
            }

            if (queryObject.EndDate != null)
            {
                query = query.Where(x => x.Date <= queryObject.EndDate);
            }

            Dictionary<string, AnalyticsObject> analytics = new Dictionary<string, AnalyticsObject>();

            query = query.Include(x => x.Category);
            foreach (var t in query)
            {
                if (analytics.ContainsKey(t.Catcode))
                {
                    analytics[t.Catcode].Amount += t.Amount;
                    analytics[t.Catcode].Count += 1;
                    if (!t.Category.ParentCode.Equals("") && String.IsNullOrEmpty(queryObject.Catcode))
                    {
                        if (analytics.ContainsKey(t.Category.ParentCode))
                        {
                            analytics[t.Category.ParentCode].Amount += t.Amount;
                            analytics[t.Category.ParentCode].Count += 1;
                        }
                        else
                        {
                            var obj = new AnalyticsObject();
                            obj.Catcode = t.Category.ParentCode; obj.Amount = t.Amount; obj.Count = 1;
                            analytics.Add(t.Catcode, obj);
                        }
                    }
                } else
                {
                    var obj = new AnalyticsObject();
                    obj.Catcode = t.Catcode; obj.Amount = t.Amount; obj.Count = 1;
                    analytics.Add(t.Catcode, obj);

                    if (!t.Category.ParentCode.Equals("") && String.IsNullOrEmpty(queryObject.Catcode))
                    {
                        if (analytics.ContainsKey(t.Category.ParentCode))
                        {
                            analytics[t.Category.ParentCode].Amount += t.Amount;
                            analytics[t.Category.ParentCode].Count += 1;
                        }
                        else
                        {
                            obj = new AnalyticsObject();
                            obj.Catcode = t.Category.ParentCode; obj.Amount = t.Amount; obj.Count = 1;
                            analytics.Add(t.Catcode, obj);
                        }
                    }
                }
            }

            if (setTop.Count() > 0)
            {
                var res = analytics.Select(pair => pair.Value).Where(x => setTop.Contains(x.Catcode)).ToList(); // samo top level
                // treba svrstati i one transakcije koje nemaju kategoriju
                var uncategorized = await _dbContext.Transactions.Where(x => String.IsNullOrEmpty(x.Catcode)).GroupBy(x => x.Catcode).Select(g => new AnalyticsObject
                {
                    Catcode = "uncategorized",
                    Amount = g.Sum(e => e.Amount),
                    Count = g.Count()
                }).ToListAsync();

                return res.Concat(uncategorized).ToList();
            }
            else
            {
                var res = analytics.Select(pair => pair.Value).ToList();
                return res;
            }
        }
    }
}
