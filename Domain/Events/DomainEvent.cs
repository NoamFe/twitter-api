using SlimMessageBus;

namespace Twitter.Domain.Events;

public abstract class DomainEvent : IRequestMessage<bool>
{
    public Guid Id { get; set; }
}
