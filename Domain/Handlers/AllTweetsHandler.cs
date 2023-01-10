using SlimMessageBus;
using Twitter.Data;
using Twitter.Domain.Events;

namespace Twitter.Domain.Handlers;

public class AllTweetsHandler : IConsumer<TweetsReceived>
{
    private readonly IWriteRepository _writeRepository;

    public AllTweetsHandler(IWriteRepository writeRepository)
    {
        _writeRepository = writeRepository;
    }


    public Task OnHandle(TweetsReceived message)
    {
        _writeRepository.AddTweets(message.Tweets);

        return Task.CompletedTask;
    }
}
