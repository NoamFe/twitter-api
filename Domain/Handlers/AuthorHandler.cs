using SlimMessageBus;
using System.Collections.ObjectModel;
using Twitter.Data;
using Twitter.Domain.Events;

namespace Twitter.Domain.Handlers;

public class AuthorHandler : IConsumer<TweetsReceived>
{
    private readonly IWriteRepository _writeRepository;

    public AuthorHandler(IWriteRepository writeRepository)
    {
        _writeRepository = writeRepository;
    }


    public Task OnHandle(TweetsReceived message)
    {
        var tweets = new Dictionary<string, List<string>>();

        foreach (var tweet in message.Tweets)
        {
            if (tweet != null && !string.IsNullOrEmpty(tweet.AuthorId))
            { 
                if (tweets.ContainsKey(tweet.AuthorId))
                {
                    tweets[tweet.AuthorId].Add(tweet.Id);
                }
                else
                {
                    tweets.Add(tweet.AuthorId, new List<string>
                            {
                                tweet.Id
                            });
                }
            }
        }
         
        _writeRepository.UpdateAuthorTweets(
            new ReadOnlyCollection<AuthorTweetsUpdateRequest>(tweets?.Select(e =>
                   new AuthorTweetsUpdateRequest(e.Key, e.Value)).ToList()));

        return Task.CompletedTask;
    }
}
