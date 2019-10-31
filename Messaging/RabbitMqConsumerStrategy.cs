using System;
using EasyNetQ;

namespace Bisk.Messaging
{
    public class RabbitMqConsumerStrategy : IDisposable, IConsumerStrategy
    {
        static string RABBIT_HOST = "rabbitmq";
        private IBus _bus;

        public RabbitMqConsumerStrategy()
        {
            RABBIT_HOST = Environment.GetEnvironmentVariable("RABBIT_HOST") ?? RABBIT_HOST;
            Console.WriteLine($"Using RabittMQ at {RABBIT_HOST}");
            _bus = RabbitHutch.CreateBus($"host={RABBIT_HOST}");
        }

        public void Subscribe<TKey, TMessage>(string subscriptionId, Action<TKey, TMessage> messageHandler) where TMessage : class
        {
            _bus.Subscribe<TMessage>(subscriptionId, message => {
                messageHandler(default(TKey), message);
            });
        }

        public void Dispose()
        {
            if (_bus != null)
                _bus.Dispose();
        }
    }
}