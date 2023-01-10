using System.Text.Json.Serialization;

namespace Twitter.DataConsumer;

public class Hashtag
{

    [JsonPropertyName("tag")]
    public string Tag { get; set; }
}
