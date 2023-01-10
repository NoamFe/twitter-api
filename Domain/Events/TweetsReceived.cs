using Twitter.DataConsumer;

namespace Twitter.Domain.Events;

public class TweetsReceived : DomainEvent
{
    public DateTime ReceiveDate { get; set; }

    public List<Tweet> Tweets { get; set; }
}