using System;
using System.ComponentModel.DataAnnotations;

namespace Xend.Models
{
    public class Client
    {
        [Key]
        public string ClientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }

}

