namespace Twitter.Models
{ 
    public class TopHashTagResponseModel
    {
        public string Tag { get; set; }
        public int Count { get; set; }
    }

    public record LanguageTweetsResponseModel(string Language, int Count);
}
