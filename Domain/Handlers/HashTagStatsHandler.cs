using SlimMessageBus;
using Twitter.Data;
using System.Collections.ObjectModel;
using Twitter.Domain.Events;

namespace Twitter.Domain.Handlers;

public class HashTagStatsHandler : IConsumer<TweetsReceived>
{
    private readonly IWriteRepository _writeRepository;

    public HashTagStatsHandler(IWriteRepository writeRepository)
    {
        _writeRepository = writeRepository;
    }

    private class HashTagTweet
    {  
        public List<string> TweetIds { get; set; }

    }

    public Task OnHandle(TweetsReceived message)
    {
        var hashtagsAndTweetIds = new Dictionary<string, List<string>>();

        foreach (var tweet in message.Tweets)
        {
            if (tweet != null && tweet.Entities?.Hashtags?.Length > 0)
            {
                foreach (var hashtag in tweet.Entities?.Hashtags)
                {
                    if (hashtagsAndTweetIds.ContainsKey(hashtag.Tag))
                    {
                        hashtagsAndTweetIds[hashtag.Tag].Add(tweet.Id);
                    }
                    else
                    {
                        hashtagsAndTweetIds.Add(hashtag.Tag, new List<string>
                            {
                                tweet.Id
                            });
                    }
                }
            }
        }
         
        _writeRepository.AddHashTags(new ReadOnlyCollection<HashTagsTweetsUpdateRequest>(
          hashtagsAndTweetIds?.Select(e =>
                 new HashTagsTweetsUpdateRequest(e.Key, e.Value)).ToList()));
         
        return Task.CompletedTask;
    }
}