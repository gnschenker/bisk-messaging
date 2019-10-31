using Bisk.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Test.Messages;

namespace TestPublisher
{
    class Program
    {
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);
        private static readonly Random rnd = new Random((int)DateTime.Now.Ticks);
        static void Main(string[] args)
        {
            Task.Factory.StartNew(() =>
            {
                Publishing(args);
            });
            Console.WriteLine("Publishing messages...");
            Console.WriteLine(" Press CTRL-c to exit.");
            Console.CancelKeyPress += (sender, args) =>
            {
                Console.WriteLine("Exit");
                _closing.Set();
            };
            _closing.WaitOne();
        }

        private static void Publishing(string[] args)
        {
            Console.WriteLine(">>>START Publishing");
            using (var strategy = new RabbitMqPublisherStrategy())
            {
                while (true)
                {
                    var message = GetRandomMessage();
                    strategy.Publish(string.Empty, message);
                    Thread.Sleep(500);
                }
            }
        }

        private static TextMessage GetRandomMessage()
        {
            var points = new String('.', rnd.Next(20));
            var message = new TextMessage
            {
                Text = $"Hello Pluggable World{points}"
            };
            return message;
        }
    }
}
