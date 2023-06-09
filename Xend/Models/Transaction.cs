using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Xend.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string WalletAddress { get; set; }
        public string CurrencyType { get; set; }
        public double Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public Client Client { get; set; }
    }
}

