using System;
namespace Xend.Models
{
    public class UpdateTransactionsCommand
    {
        public string ClientId { get; set; }
        public string WalletAddress { get; set; }
        public string CurrencyType { get; set; }
    }

}

