using System.Threading.Tasks;

namespace Bisk.Messaging
{
    public interface IAsyncPublisherStrategy
    {
        Task PublishAsync<TKey, TMessage>(TKey key, TMessage message) where TMessage : class;
    }
}
