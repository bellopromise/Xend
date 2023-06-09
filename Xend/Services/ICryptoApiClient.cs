using System;
using Xend.Models;

namespace Xend.Services
{
    public interface ICryptoApiClient
    {
        Task<IEnumerable<Transaction>> GetTransactions(string walletAddress, string currencyType);
    }

}

