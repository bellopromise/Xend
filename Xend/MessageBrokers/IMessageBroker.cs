using System;
namespace Xend.MessageBrokers
{
    public interface IMessageBroker
    {
        void Publish<TMessage>(TMessage message);
    }

}

