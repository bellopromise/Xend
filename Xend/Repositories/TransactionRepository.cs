using System;
using Microsoft.EntityFrameworkCore;
using Xend.Data;
using Xend.Models;

namespace Xend.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IClientRepository _clientRepository;

        public TransactionRepository(ApplicationDbContext dbContext, IClientRepository clientRepository)
        {
            _dbContext = dbContext;
            _clientRepository = clientRepository;
        }

        public void SaveTransactions(string clientId, IEnumerable<Transaction> transactions)
        {
            var client = _clientRepository.GetClientByClientIdAsync(clientId).GetAwaiter().GetResult();

            if (client == null)
            {
                throw new Exception($"Client with ID '{clientId}' does not exist.");
            }

            var transactionEntities = transactions.Select(t => new Transaction
            {
                ClientId = clientId,
                WalletAddress = t.WalletAddress,
                CurrencyType = t.CurrencyType,
                Amount = t.Amount,
                Client = client// Map other transaction properties as needed
            });

            _dbContext.AddRange(transactionEntities);
            _dbContext.SaveChanges();
        }

        public void SaveTransaction(Transaction transaction)
        {
            
            _dbContext.Add(transaction);
            _dbContext.SaveChanges();
        }



        public IEnumerable<Transaction> GetTransactionsByClientId(string clientId)
        {
            // Assuming you have an entity model called "TransactionEntity" representing transactions in the database
            var transactionEntities = _dbContext.Set<Transaction>()
                .Where(t => t.ClientId == clientId)
                .ToList();

            // Map transaction entities to domain model transactions
            var transactions = transactionEntities.Select(t => new Transaction
            {
                Id = t.Id
                // Map other transaction properties as needed
            });

            return transactions;
        }

        public async Task<bool> IsDuplicateRequestAsync(string clientId, string walletAddress, string currencyType)
        {
            // Check if there is a transaction with the same ClientId, WalletAddress, and CurrencyType
            bool isDuplicate = await _dbContext.Transactions.AnyAsync(t =>
                t.ClientId == clientId &&
                t.WalletAddress == walletAddress &&
                t.CurrencyType == currencyType);

            return isDuplicate;
        }
    }

}

