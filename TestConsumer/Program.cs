using Bisk.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Test.Messages;

namespace TestConsumer
{
    class Program
    {
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            Task.Factory.StartNew(() =>
            {
                Consuming();
            });
            Console.WriteLine("Waiting for messages...");
            Console.WriteLine(" Press CTRL-c to exit.");
            Console.CancelKeyPress += (sender, _) => {
                Console.WriteLine("Exiting...");
                _closing.Set();
            };
            _closing.WaitOne();
        }
        private static void Consuming()
        {
            Console.WriteLine(">>> Start consuming!");
            var consumer = new RabbitMqConsumerStrategy();
            consumer.Subscribe<string, TextMessage>("test-consumer", (key, message) =>
            {
                Console.WriteLine($"Got key: {key}, message: {message}");
            });
            _closing.WaitOne();
            consumer.Dispose();
            Console.WriteLine("<<< End consuming!");
        }
    }
}
