using System;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace Xend.MessageBrokers
{
    public class RabbitMQMessageBroker : IMessageBroker
    {
        private readonly IModel _channel;
        private readonly string _exchangeName;

        public RabbitMQMessageBroker(IModel channel, string exchangeName)
        {
            _channel = channel;
            _exchangeName = exchangeName;

            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout);
        }

        public void Publish<TMessage>(TMessage message)
        {
            var body = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            _channel.BasicPublish(_exchangeName, "", null, body);
        }
    }
}

