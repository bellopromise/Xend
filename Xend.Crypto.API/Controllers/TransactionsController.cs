using System;
using Microsoft.AspNetCore.Mvc;
using Xend.Crypto.API.Models;

namespace Xend.Crypto.API.Controllers
{
    [ApiController]
    [Route("crypto-api/transactions")]
    public class TransactionsController : ControllerBase
    {
        // GET: api/transactions?walletAddress={walletAddress}&currencyType={currencyType}
        [HttpGet]
        public ActionResult<IEnumerable<Transaction>> GetTransactions(string walletAddress, string currencyType)
        {
            // Retrieve transactions based on the provided wallet address and currency type
            // You can use a mock data source or generate dummy transactions for testing purposes

            // Example response
            var transactions = new List<Transaction>
        {
            new Transaction { Id = 1, WalletAddress = walletAddress, CurrencyType = currencyType, Amount = 10.0m, Timestamp = DateTime.Now },
            new Transaction { Id = 2, WalletAddress = walletAddress, CurrencyType = currencyType, Amount = 20.0m, Timestamp = DateTime.Now },
            new Transaction { Id = 3, WalletAddress = walletAddress, CurrencyType = currencyType, Amount = 30.0m, Timestamp = DateTime.Now },
            new Transaction { Id = 4, WalletAddress = walletAddress, CurrencyType = currencyType, Amount = 40.0m, Timestamp = DateTime.Now },
            new Transaction { Id = 5, WalletAddress = walletAddress, CurrencyType = currencyType, Amount = 50.0m, Timestamp = DateTime.Now }
        };

            return Ok(transactions);
        }
    }

}

