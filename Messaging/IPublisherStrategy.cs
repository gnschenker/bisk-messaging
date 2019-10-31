using System;

namespace Bisk.Messaging
{
    public interface IPublisherStrategy : IDisposable
    {
        void Publish<TKey, TMessage>(TKey key, TMessage message) where TMessage : class;
    }
}