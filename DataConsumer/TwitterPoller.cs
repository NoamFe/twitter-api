
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using RestSharp;
using SlimMessageBus;
using System.Text;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;
using SlimMessageBus;
using SlimMessageBus.Host.AspNetCore;
using SlimMessageBus.Host;
using Twitter.Domain.Events;

namespace Twitter.DataConsumer;

public class Data
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public sealed class PollerBackgroundService : BackgroundService
{
    private int _batchSize = 50;
    private string mediaType = null;
    private readonly ILogger<PollerBackgroundService> _logger;
    private readonly IPublishBus _publisher;
    private readonly HttpClient _client;
    private readonly string _url;
    private readonly string _bearer;

    public PollerBackgroundService(IHttpClientFactory factory,
        IConfiguration configuration,
        ILogger<PollerBackgroundService> logger,
         IPublishBus publisher)
    {
        _logger = logger;
        _publisher = publisher;
        _client = factory.CreateClient();

        _batchSize = Int32.Parse(configuration["TwitterBatchSize"]);
        _url = configuration["TwitterURL"];
        _bearer = configuration["TwitterBearer"];
         
        _client.Timeout = new TimeSpan(0, 0, 30);
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _bearer);
    }


    private async Task<Tweet> ReadTweetAsync(StreamReader reader)
    {
        try
        {
            string? json = await reader.ReadLineAsync();
            if (!string.IsNullOrEmpty(json))
            {
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

                var tweet = await System.Text.Json.JsonSerializer.DeserializeAsync<TweetData>(stream);
                return tweet.Tweet;
            }

            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failure in ReadTweet:", ex);

            return default;
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation($"{DateTime.Now}.... establishing connection to Twitter. please wait ....");

            var client = new RestClient(_url);
            var request = new RestRequest
            {
                Method = Method.Get,
            };
            request.AddHeader("Authorization", $"Bearer {_bearer}");

            request.AddHeaders(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Connection", "keep-alive"),
                new KeyValuePair<string, string>("Accept", "*/*"),
            });

            var tweets = new List<Tweet>(_batchSize);

            var batchCounter = 0;
            using (var stream = await client.DownloadStreamAsync(request))
            {

                _logger.LogInformation($"{DateTime.Now}.... Reading stream from twitter....");
                 
                if (stream != null && stream.CanRead)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        int index = 0;

                        while (reader != null && !reader.EndOfStream)
                        {
                            tweets.Add(await ReadTweetAsync(reader));

                            index++;
                            batchCounter++;

                            _logger.LogInformation($"{DateTime.Now}.... Reading a tweet batch#{batchCounter} total#:{index}");


                            if (batchCounter == _batchSize)
                            {
                                var publishTask = _publisher.Publish(
                                    new TweetsReceived
                                    {
                                        Tweets = tweets,
                                        ReceiveDate = DateTime.UtcNow
                                    });

                                //Only using this way because this current slimBus is blocking
                                Task.Factory.StartNew(async () => await publishTask);

                                _logger.LogInformation($"publishing {batchCounter} of tweets..." );

                                batchCounter = 0;

                                tweets.Clear();

                            }
                        }
                    }
                }


                while (!stoppingToken.IsCancellationRequested)
                {//https://api.twitter.com/2/tweets/sample/stream

                    string apiPath = "/2/tweets/sample/stream";

                    var jsonResult = await _client.GetStringAsync(apiPath);

                    var tweetsList =
                        JsonConvert.DeserializeObject<List<Data>>(jsonResult,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                    // string joke = _jokeService.GetJoke();
                    _logger.LogWarning("{Joke}");//, joke);

                    await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);

            // Terminates this process and returns an exit code to the operating system.
            // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
            // performs one of two scenarios:
            // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
            // 2. When set to "StopHost": will cleanly stop the host, and log errors.
            //
            // In order for the Windows Service Management system to leverage configured
            // recovery options, we need to terminate the process with a non-zero exit code.
            // Environment.Exit(1);
        }
    }
}
