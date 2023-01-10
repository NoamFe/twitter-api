using Twitter.DataConsumer;
using Twitter.Models;

namespace Twitter.Data;

public interface IReadRepository
{
    int GetTotalTweetCount();

    IEnumerable<TopHashTagResponseModel> GetTopHashTags(int take);

    IEnumerable<Tweet> GetTweetsByHashtag(string tag);

    IEnumerable<LanguageTweetsResponseModel> GetLanguageCount();

    IEnumerable<Tweet> GetLanguageCount(string langaugeCode, int take, int skip);
    IEnumerable<Tweet> GetByAuthorCount(string authorId, int take, int skip);
    
    Tweet GetTweet(string id);
}
