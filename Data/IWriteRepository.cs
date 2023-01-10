using Twitter.DataConsumer;

namespace Twitter.Data;

public interface IWriteRepository
{
    void AddHashTags(IReadOnlyCollection<HashTagsTweetsUpdateRequest> requests);
    void AddTweets(List<Tweet> tweets);
    int TweetsCount { get; }
    void UpdateLanguageTweets(IReadOnlyCollection<LangageTweetsUpdateRequest> requests);
    void UpdateAuthorTweets(IReadOnlyCollection<AuthorTweetsUpdateRequest> requests);
    
}
