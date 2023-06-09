using System;
using Xend.EventHandlers;

namespace Xend.Models
{
    public class TransactionReceivedEvent : IEvent
    {
        public string ClientId { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
    }

}

