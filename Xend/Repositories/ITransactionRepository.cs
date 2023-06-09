using System;
using Xend.Models;

namespace Xend.Repositories
{
    public interface ITransactionRepository
    {
        void SaveTransactions(string clientId, IEnumerable<Transaction> transactions);
        IEnumerable<Transaction> GetTransactionsByClientId(string clientId);
        public void SaveTransaction(Transaction transaction);
        Task<bool> IsDuplicateRequestAsync(string clientId, string walletAddress, string currencyType);
    }

}

