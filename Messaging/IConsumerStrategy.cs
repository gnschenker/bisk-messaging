using System;

namespace Bisk.Messaging
{
    public interface IConsumerStrategy : IDisposable
    {
        void Subscribe<TKey, TMessage>(string subscriptionId, Action<TKey, TMessage> handler) where TMessage : class;
    }
}