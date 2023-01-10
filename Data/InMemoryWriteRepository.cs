using Twitter.DataConsumer;

namespace Twitter.Data;

public class InMemoryWriteRepository : IWriteRepository
{
    public void AddTweets(List<Tweet> tweets)
    {
        Parallel.ForEach(tweets, tweet =>
        {
            if(tweet!=null)
                InMemoryDB.AllTweets.AddOrUpdate(tweet.Id, tweet, (key, current) => tweet);

        });
    }

    public void UpdateLanguageTweets(IReadOnlyCollection<LangageTweetsUpdateRequest> requests)
    {
        Parallel.ForEach(requests, request =>
        {
            InMemoryDB.LangaugeTweets.TryGetValue(request.Language, out var tweetIds);

            (tweetIds ??= new List<string>()).AddRange(request.TweetIds);

            InMemoryDB.LangaugeTweets.AddOrUpdate(request.Language, tweetIds,
               (key, current) => tweetIds);
        });
    }

    public void AddHashTags2(IReadOnlyCollection<HashTagsTweetsUpdateRequest> requests)
    {
        Parallel.ForEach(requests, request =>
        {

            InMemoryDB.HashtagsCount.TryGetValue(request.Hashtag, out var value);
            value += request.TweetIds.Count;

            InMemoryDB.HashtagsCount.AddOrUpdate(request.Hashtag, value, (key, current) => value);


            InMemoryDB.HashTagTweets.TryGetValue(request.Hashtag, out var tweetIds);

            (tweetIds ??= new List<string>()).AddRange(request.TweetIds);

            InMemoryDB.HashTagTweets.AddOrUpdate(request.Hashtag, tweetIds, (key, current) => tweetIds);
        });
    }


    public static int addition(int a,int b) => (a+b);

    public void AddHashTags(IReadOnlyCollection<HashTagsTweetsUpdateRequest> requests) 
    {
        Parallel.ForEach(requests, request =>
        {
            Func<int, int, int> add = (int a, int b) => (a + b);

            if (InMemoryDB.HashtagsCount.TryGetValue(request.Hashtag, out var value))
            { 
                InMemoryDB.HashtagsCount.TryUpdate(request.Hashtag, request.TweetIds.Count, add);
            }
            else 
            {
                InMemoryDB.HashtagsCount.TryAdd(request.Hashtag, request.TweetIds.Count, add);
            }

            /* InMemoryDB.HashtagsCount.TryUpdate(request.Hashtag, value2, value2 + request.TweetIds.Count);




             InMemoryDB.HashtagsCount.TryGetValue(request.Hashtag, out var value);
             value += request.TweetIds.Count;

             InMemoryDB.HashtagsCount.AddOrUpdate(request.Hashtag, value, (key, current) => value);


             InMemoryDB.HashTagTweets.TryGetValue(request.Hashtag, out var tweetIds);

             (tweetIds ??= new List<string>()).AddRange(request.TweetIds);

             InMemoryDB.HashTagTweets.AddOrUpdate(request.Hashtag, tweetIds, (key, current) => tweetIds);*/

            Func<List<string>, List<string>, List<string>> combineLists
            = (List<string> a, List<string> b) =>
            {
                if (a == null && b==null)
                    return new List<string>();

                if (a == null)
                    return b;

                if (b == null)
                    return a;

                a.AddRange(b);
                return a;
            };

            if (InMemoryDB.HashTagTweets.TryGetValue(request.Hashtag, out var tweetIds))
            {
                InMemoryDB.HashTagTweets.TryUpdate(request.Hashtag, request.TweetIds, combineLists);
            }
            else
            {
                InMemoryDB.HashTagTweets.TryAdd(request.Hashtag, request.TweetIds, combineLists);
            }


        }); 
    }
     

    public void UpdateAuthorTweets(IReadOnlyCollection<AuthorTweetsUpdateRequest> requests)
    {
        Parallel.ForEach(requests, request =>
        {
            InMemoryDB.AuthorTweets.TryGetValue(request.AuthorId, out var tweetIds);

            (tweetIds ??= new List<string>()).AddRange(request.TweetIds);

            InMemoryDB.AuthorTweets.AddOrUpdate(request.AuthorId, tweetIds,
               (key, current) => tweetIds);
        });
    }

    public int TweetsCount => InMemoryDB.AllTweets.Keys.Count;
     
}
