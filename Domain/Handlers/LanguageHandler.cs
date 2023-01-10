using SlimMessageBus;
using System.Collections.ObjectModel;
using Twitter.Data;
using Twitter.Domain.Events;

namespace Twitter.Domain.Handlers;

public class LanguageHandler : IConsumer<TweetsReceived>
{
    private readonly IWriteRepository _writeRepository;

    public LanguageHandler(IWriteRepository writeRepository)
    {
        _writeRepository = writeRepository;
    }


    public Task OnHandle(TweetsReceived message)
    {
        var langaugeTweets = new Dictionary<string, List<string>>();

        foreach (var tweet in message.Tweets)
        {
            if (tweet != null && !string.IsNullOrEmpty(tweet.Language))
            { 
                if (langaugeTweets.ContainsKey(tweet.Language))
                {
                    langaugeTweets[tweet.Language].Add(tweet.Id);
                }
                else
                {
                    langaugeTweets.Add(tweet.Language, new List<string>
                            {
                                tweet.Id
                            });
                }
            }
        }
  
        _writeRepository.UpdateLanguageTweets(new ReadOnlyCollection<LangageTweetsUpdateRequest>(
            langaugeTweets?.Select(e =>
                   new LangageTweetsUpdateRequest(e.Key, e.Value)).ToList()));

        return Task.CompletedTask;
    }
}
