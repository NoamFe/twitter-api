using System.Text.Json.Serialization;

namespace Twitter.DataConsumer;

public class TweetData
{
    [JsonPropertyName("data")]
    public Tweet Tweet { get; set; }
}
