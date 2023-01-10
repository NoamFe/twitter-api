using System.Text.Json.Serialization;

namespace Twitter.DataConsumer;

public class Entities
{
    //  [JsonPropertyName("annotations")]
    //  public Annotation[] Annotations { get; set; }
    // [JsonPropertyName("cashtags")]
    // public Cashtag[] Cashtags { get; set; }

    [JsonPropertyName("hashtags")]
    public Hashtag[] Hashtags { get; set; }

    //  [JsonPropertyName("mentions")]
    ///public Mention[] Mentions { get; set; }

    //  [JsonPropertyName("urls")]
    // public Url[] Urls { get; set; }

}
