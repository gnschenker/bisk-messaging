using System;
using System.Threading.Tasks;
using EasyNetQ;

namespace Bisk.Messaging
{
    public class RabbitMqAsyncConsumerStrategy : IDisposable, IAsyncConsumerStrategy
    {
        static string RABBIT_HOST = "rabbitmq";
        private IBus _bus;

        public RabbitMqAsyncConsumerStrategy()
        {
            RABBIT_HOST = Environment.GetEnvironmentVariable("RABBIT_HOST") ?? RABBIT_HOST;
            Console.WriteLine($"Using RabittMQ at {RABBIT_HOST}");
            _bus = RabbitHutch.CreateBus($"host={RABBIT_HOST}");
        }

        public Task SubscribeAsync<TKey, TMessage>(string subscriptionId, Action<TKey, TMessage> handler) where TMessage : class
        {
            Task task = null;
            _bus.SubscribeAsync<TMessage>(subscriptionId, message => task = Task.Run(() => handler(default(TKey), message)));
            return task;
        }

        public void Dispose()
        {
            if (_bus != null)
                _bus.Dispose();
        }
    }
}