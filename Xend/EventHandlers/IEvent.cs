using System;
using Xend.Models;

namespace Xend.EventHandlers
{
    public interface  IEvent
    {
        string ClientId { get; set; }
        IEnumerable<Transaction> Transactions { get; set; }
    }

}

