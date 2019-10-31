using System;
using System.Threading.Tasks;
using EasyNetQ;

namespace Bisk.Messaging
{
    public class RabbitMqAsyncPublisherStrategy : IDisposable, IAsyncPublisherStrategy
    {
        static string RABBIT_HOST = "rabbitmq";
        static bool RABBIT_PUBLISHER_CONFIRM = true;
        static int RABBIT_TIMEOUT = 10;
        private IBus _bus;

        public RabbitMqAsyncPublisherStrategy()
        {
            RABBIT_HOST = Environment.GetEnvironmentVariable("RABBIT_HOST") ?? RABBIT_HOST;
            RABBIT_PUBLISHER_CONFIRM = bool.TryParse(Environment.GetEnvironmentVariable("RABBIT_PUBLISHER_CONFIRM"), out bool confirm) ? confirm : RABBIT_PUBLISHER_CONFIRM;
            RABBIT_TIMEOUT = int.TryParse(Environment.GetEnvironmentVariable("RABBIT_TIMEOUT"), out int timeout) ? timeout : RABBIT_TIMEOUT;
            Console.WriteLine($"Using RabittMQ at {RABBIT_HOST}, publisher confirm = {RABBIT_PUBLISHER_CONFIRM}, timeout = {RABBIT_TIMEOUT}");
            _bus = RabbitHutch.CreateBus($"host={RABBIT_HOST},publisherConfirm={RABBIT_PUBLISHER_CONFIRM},timeout={RABBIT_TIMEOUT}");
        }

        public Task PublishAsync<TKey, TMessage>(TKey key, TMessage message) where TMessage : class
        {
            return _bus.PublishAsync(message);
        }

        public void Dispose()
        {
            if (_bus != null)
                _bus.Dispose();
        }
    }
}