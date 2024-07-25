﻿using ProductAPI.Database.Entities;
using ProductAPI.Models;
using ProductAPI.Objects;

namespace ProductAPI.Database.Repositories
{
    public interface ITransactionRepository
    {
        Task<TransactionEntity> InsertTransactionAsync(TransactionEntity transaction);

        Task<int> InsertBulkTransactions(List<TransactionEntity> transactions);

        Task<PagedSortedList<TransactionEntity>> GetTransactionsAsync(QueryObject queryObject);

        Task DeleteTransactionAsync(TransactionEntity transaction);

        Task<TransactionEntity> GetTransactionByIdAsync(string transactionId);
    }
}
