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
        private static readonly Random rnd = new Random((int)DateTime.Now.Ticks);

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
            var consumer = new RabbitMqAsyncConsumerStrategy();
            consumer.SubscribeAsync<string, TextMessage>("test-consumer", (key, message) => 
            {
                Thread.Sleep(rnd.Next(50, 2000));   // sleep between 50 ms and 2 sec 
                Console.WriteLine($"ASYNC - Got key: {key}, message: {message}");
            });
            _closing.WaitOne();
            consumer.Dispose();
            Console.WriteLine("<<< End consuming!");
        }
    }
}
