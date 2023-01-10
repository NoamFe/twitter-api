using System.Text.Json.Serialization;

namespace Twitter.DataConsumer;

public class Tweet
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("edit_history_tweet_ids")]
    public string[] EditHistoryTweetIds { get; set; }

    [JsonPropertyName("entities")]
    public Entities Entities { get; set; }

    [JsonPropertyName("attachments")]
    public dynamic Attachments { get; set; }

    [JsonPropertyName("author_id")]
    public string AuthorId { get; set; }

    [JsonPropertyName("context_annotations")]
    public dynamic ContextAnnotations { get; set; }

    [JsonPropertyName("conversation_id")]
    public string ConversationId { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("in_reply_to_user_id")]
    public string InReplyToUserId { get; set; }

    [JsonPropertyName("lang")]
    public string Language { get; set; }

    [JsonPropertyName("referenced_tweets")]
    public dynamic[] ReferencedTweets { get; set; }

    [JsonPropertyName("reply_settings")]
    public string ReplySettings { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("withheld")]
    public dynamic Withheld { get; set; }
}
