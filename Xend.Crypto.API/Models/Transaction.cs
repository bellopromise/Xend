using System;
namespace Xend.Crypto.API.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string WalletAddress { get; set; }
        public string CurrencyType { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
    }

}

