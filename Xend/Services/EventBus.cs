using System;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Xend.EventHandlers;

namespace Xend.Services
{
    public class EventBus : IEventBus, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchangeName;

        public EventBus(IConnection connection, string exchangeName)
        {
            _connection = connection;
            _channel = _connection.CreateModel();
            _exchangeName = exchangeName;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var eventName = @event.GetType().Name;
            var message = JsonConvert.SerializeObject(@event);
            var body = System.Text.Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: _exchangeName, routingKey: eventName, basicProperties: null, body: body);
        }

        public void Subscribe<TEvent, TEventHandler>() where TEvent : IEvent where TEventHandler : IEventHandler<TEvent>
        {
            var eventName = typeof(TEvent).Name;

            _channel.QueueDeclare(queue: eventName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, args) =>
            {
                var message = System.Text.Encoding.UTF8.GetString(args.Body.ToArray());
                var @event = JsonConvert.DeserializeObject<TEvent>(message);

                var handler = Activator.CreateInstance<TEventHandler>();
                handler.Handle(@event);

                _channel.BasicAck(args.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: eventName, autoAck: false, consumer: consumer);
        }

        public void Unsubscribe<TEvent, TEventHandler>() where TEvent : IEvent where TEventHandler : IEventHandler<TEvent>
        {
            var eventName = typeof(TEvent).Name;

            _channel.QueueDelete(eventName);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }



}

