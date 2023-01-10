using Twitter.DataConsumer;
using Twitter.Models;

namespace Twitter.Data;


public class InMemoryReadRepository : IReadRepository
{
    public IEnumerable<TopHashTagResponseModel> GetTopHashTags(int take)
    {
        var topHashTagsList = InMemoryDB.HashtagsCount
                                .Select(x => new { HashTag = x.Key, Count = x.Value })
                                .OrderByDescending(o => o.Count)
                                .Take(take)
                                .Select(x => 
                                new TopHashTagResponseModel
                                { 
                                 Count = x.Count,
                                 Tag = x.HashTag
                                } );

        return topHashTagsList;
    }

    public int GetTotalTweetCount() => InMemoryDB.AllTweets.Count;

    public IEnumerable<Tweet> GetTweetsByHashtag(string tag)
    {
        InMemoryDB.HashTagTweets.TryGetValue(tag, out var tweetIds);

        if (tweetIds == null)
            yield break; 

        foreach (var tweetId in tweetIds)
        {
            yield return InMemoryDB.AllTweets.TryGetValue(tweetId, out Tweet value) ? value : null;
        }
    }
     

    public IEnumerable<LanguageTweetsResponseModel> GetLanguageCount()
    { 
        foreach (var language in InMemoryDB.LangaugeTweets)
        {
            yield return new LanguageTweetsResponseModel(language.Key, language.Value.Count);
        }
    }

    public IEnumerable<Tweet> GetLanguageCount(string langaugeCode,int take, int skip)
    { 
        InMemoryDB.LangaugeTweets.TryGetValue(langaugeCode, out var tweetIds);

        if (tweetIds == null)
            yield break;

        foreach (var tweetId in tweetIds.Take(take).Skip(skip))
        { 
            InMemoryDB.AllTweets.TryGetValue(tweetId, out var value);

            yield return value;
        }
    } 
     
    public Tweet GetTweet(string id)
    {
        InMemoryDB.AllTweets.TryGetValue(id, out var tweet);

        return tweet;
    }

    public IEnumerable<Tweet> GetByAuthorCount(string authorId, int take, int skip)
    {
        InMemoryDB.AuthorTweets.TryGetValue(authorId, out var tweetIds);

        if (tweetIds == null)
            yield break;

        foreach (var tweetId in tweetIds.Take(take).Skip(skip))
        {
            InMemoryDB.AllTweets.TryGetValue(tweetId, out var value);

            yield return value;
        }
    }
}
