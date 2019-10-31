using System;
using System.Threading.Tasks;

namespace Bisk.Messaging
{
    public interface IAsyncConsumerStrategy : IDisposable
    {
        Task SubscribeAsync<TKey, TMessage>(string subscriptionId, Action<TKey, TMessage> handler) where TMessage : class;
    }
}