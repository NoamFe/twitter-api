using System.Collections.Concurrent;
using Twitter.DataConsumer;

namespace Twitter.Data;

internal static class InMemoryDB
{
    static InMemoryDB()
    {
        AllTweets = new ConcurrentDictionary<string, Tweet>();
        HashtagsCount = new ConcurrentDictionary<string, int>();
        HashTagTweets = new ConcurrentDictionary<string, List<string>>();
        LangaugeTweets = new ConcurrentDictionary<string, List<string>>();
        AuthorTweets = new ConcurrentDictionary<string, List<string>>();
    }

    public static ConcurrentDictionary<string, Tweet> AllTweets { get; set; }
    public static ConcurrentDictionary<string, int> HashtagsCount { get; set; }

    public static ConcurrentDictionary<string, List<string>> HashTagTweets { get; set; }

    public static ConcurrentDictionary<string, List<string>> LangaugeTweets { get; set; }
    public static ConcurrentDictionary<string, List<string>> AuthorTweets { get; set; }
}
