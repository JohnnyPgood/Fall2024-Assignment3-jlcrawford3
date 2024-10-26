using System.ClientModel;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using VaderSharp2;

namespace Fall2024_Assignment3_jlcrawford3.Services;

public class AIService
{
    private readonly AzureOpenAIClient _openAIClient;
    private readonly SentimentIntensityAnalyzer _sentimentAnalyzer;
    private const string AiDeployment = "gpt-35-turbo";
    private const int NumCritics = 10;
    private const int NumTweets = 20;

    public class AIServiceException : Exception
    {
        public AIServiceException(string message, Exception? innerException = null)
            : base(message, innerException) { }
    }

    public AIService(IConfiguration configuration)
    {
        _openAIClient = new AzureOpenAIClient(
            new Uri(configuration["Azure:OpenAI:Endpoint"]!),
            new ApiKeyCredential(configuration["Azure:OpenAI:Key"]!)
        );
        _sentimentAnalyzer = new SentimentIntensityAnalyzer();
    }

    public async Task<List<(string Review, double Sentiment)>> GenerateMovieReviewsAsync(string title, int year, string genre)
    {
        try
            {
            var chatClient = _openAIClient.GetChatClient(AiDeployment);
            var messages = new ChatMessage[]
            {
                new SystemChatMessage($"You are a group of {NumCritics} different film critics, one of which is extremely sarcastic. When you receive a question, respond as each critic with each response separated by a '|', but don't indicate which critic you are."),
                new UserChatMessage($"Write a short review of the {genre} movie {title} ({year}) in less than 150 words, and rate it out of 10.")
            };

            var result = await chatClient.CompleteChatAsync(messages);
            if (result?.Value?.Content == null || result.Value.Content.Count == 0)
            {
                throw new AIServiceException("No content received from AI service.");
            }

            var reviews = result.Value.Content[0].Text
                .Split('|')
                .Select(s => s.Trim())
                .Take(10)
                .ToList();
            if (!reviews.Any())
            {
                throw new AIServiceException("No valid reviews were collected.");
            }

            return reviews.Select(review => 
            {
                var sentiment = _sentimentAnalyzer.PolarityScores(review).Compound;
                return (review, sentiment);
            }).ToList();
        }
        catch (Exception ex) when (ex is not AIServiceException)
        {
            throw new AIServiceException("Failed to generate movie reviews.", ex);
        }
    }

    public async Task<List<(string Tweet, double Sentiment)>> GenerateActorTweetsAsync(string name, string gender, int age)
    {
        try
        {
            var chatClient = _openAIClient.GetChatClient(AiDeployment);
            var messages = new ChatMessage[]
            {
                new SystemChatMessage($"You are a group of {NumTweets} different social media users. When you receive a question, respond as each user with each response separated by a '|', but don't indicate which user you are."),
                new UserChatMessage($"Write a tweet about the {gender} actor {name}, who is {age} years old. Keep each tweet under 280 characters and make them sound authentic and varied in tone.")
            };

            var result = await chatClient.CompleteChatAsync(messages);
            if (result?.Value?.Content == null || result.Value.Content.Count == 0)
            {
                throw new AIServiceException("No content received from AI service.");
            }

            var tweets = result.Value.Content[0].Text
                .Split('|')
                .Select(s => s.Trim())
                .Take(20)
                .ToList();
            if (!tweets.Any())
            {
                throw new AIServiceException("No valid tweets were collected.");
            }

            return tweets.Select(tweet => 
            {
                var sentiment = _sentimentAnalyzer.PolarityScores(tweet).Compound;
                return (tweet, sentiment);
            }).ToList();
        }
        catch (Exception ex) when (ex is not AIServiceException)
        {
            throw new AIServiceException("Failed to generate actor tweets.", ex);
        }
    }

    public async Task<bool> VerifyConnectionAsync()
    {
        try
        {
            var messages = new ChatMessage[]
            {
                new UserChatMessage("Hello, this is a test message.")
            };

            var response = await _openAIClient.GetChatClient(AiDeployment).CompleteChatAsync(
                messages
            );
            Console.WriteLine($"OpenAI connection test response: {response.Value.Content[0].Text}");

            return response.Value.Content.Count > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"OpenAI connection test failed: {ex.Message}");
            return false;
        }
    }
}
