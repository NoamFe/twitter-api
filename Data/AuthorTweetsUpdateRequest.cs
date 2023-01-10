namespace Twitter.Data
{ 
    public record AuthorTweetsUpdateRequest(string AuthorId, List<string> TweetIds);
} 
    