using System.Reflection.Metadata.Ecma335;
using Twitter.DataConsumer;
using Twitter.Domain.Events;

namespace Twitter.Bus
{
    public interface IBus
    {
        Task PublishAsync<T>(T @event) where T : DomainEvent;
    }

    public class Bus : IBus
    {
        private readonly SlimMessageBus.IPublishBus _publisher;
        public Bus(SlimMessageBus.IPublishBus publisher)
        {
            _publisher = publisher;
        }

        public async Task PublishAsync<T>(T @event) where T : DomainEvent
        {
             await _publisher.Publish(@event);
        }
  
    }
}
