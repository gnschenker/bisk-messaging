using System;
using EasyNetQ;

namespace Bisk.Messaging
{
    public class RabbitMqPublisherStrategy : IDisposable, IPublisherStrategy
    {
        static string RABBIT_HOST = "rabbitmq";
        private IBus _bus;

        public RabbitMqPublisherStrategy()
        {
            RABBIT_HOST = Environment.GetEnvironmentVariable("RABBIT_HOST") ?? RABBIT_HOST;
            Console.WriteLine($"Using RabittMQ at {RABBIT_HOST}");
            _bus = RabbitHutch.CreateBus($"host={RABBIT_HOST}");
        }

        public void Publish<TKey, TMessage>(TKey key, TMessage message) where TMessage : class
        {
            _bus.Publish(message);
            Console.WriteLine($"Sent '{message}'");
        }

        public void Dispose()
        {
            if (_bus != null)
                _bus.Dispose();
        }
    }
}