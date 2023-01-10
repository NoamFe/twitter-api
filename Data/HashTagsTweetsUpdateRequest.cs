namespace Twitter.Data
{
    public record HashTagsTweetsUpdateRequest(string Hashtag, List<string> TweetIds);
} 
    