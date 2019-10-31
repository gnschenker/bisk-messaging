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
            using (var strategy = new RabbitMqAsyncPublisherStrategy())
            {
                var count=1;
                while (true)
                {
                    var message = GetRandomMessage(count);
                    strategy.PublishAsync(string.Empty, message)
                        .ContinueWith(task =>
                        {
                            if(task.IsCompleted)
                            {
                                Console.WriteLine($"ASYNC - *** {message} ({count}) completed");
                            }
                            if(task.IsFaulted)
                            {
                                Console.Out.WriteLine("\n\n");
                                Console.Out.WriteLine(task.Exception);
                                Console.Out.WriteLine("\n\n");
                            }
                        });
                    Thread.Sleep(500);
                    count++;
                }
            }
        }

        private static TextMessage GetRandomMessage(int count)
        {
            var points = new String('.', rnd.Next(20));
            var message = new TextMessage
            {
                Text = $"Hello Pluggable World ({count}){points}"
            };
            return message;
        }
    }
}
