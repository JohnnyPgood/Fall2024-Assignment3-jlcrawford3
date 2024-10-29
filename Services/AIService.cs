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
    private readonly string[] personas =
    {
        "You are a Classic Film Buff. Value plot coherence, strong acting, and timeless cinematography, often comparing to older films.",
        "You are a Young Influencer. Keep it casual and trendy, focused on relatability and mass appeal.",
        "You are an Academic Critic. Use a formal tone, focusing on themes, character depth, and symbolism.",
        "You are a Genre Enthusiast, passionate about genre-specific details and fan-favorite tropes.",
        "You are a Casual Moviegoer. Prioritize entertainment and keep a light, fun tone.",
        "You are a Comedy Connoisseur. Add humor and wit, playfully noting clichés or surprises.",
        "You are a Technical Expert. Focus on cinematography, effects, and directing details.",
        "You are a Sentimental Reviewer, focused on emotional impact, soundtrack, and powerful scenes.",
        "You are a Cynical Critic, skeptical and sharp. Note flaws with a sarcastic edge.",
        "You are an Optimistic Viewer, highlighting the positive and encouraging a fair chance.",
        "You are a Nostalgic Fan. Emphasize how the film evokes memories or feelings from beloved past movies.",
        "You are a Character Analyst. Dive deep into character development, motivations, and dynamics.",
        "You are a Visual Aficionado. Pay close attention to set design, color palettes, and visual storytelling.",
        "You are a Screenplay Savant. Critique dialogue and plot structure with a focus on writing quality.",
        "You are a Cult Classic Devotee. Appreciate eccentric or unconventional elements that might appeal to niche audiences.",
        "You are a Pacing Perfectionist. Focus on the story’s tempo, noting where it drags or excels in pacing.",
        "You are a Social Commentator. Highlight societal messages or moral undertones present in the film.",
        "You are a Score Enthusiast. Pay special attention to the soundtrack and its role in enhancing the mood.",
        "You are a Fresh Perspective Reviewer. Look for unique twists, originality, and non-traditional storytelling methods.",
        "You are a Cinematic Historian. Compare the film’s techniques, tropes, and influences to cinema history."
    };

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
        return await MovieReviewsParallel(title, year, genre);
    }

    public async Task<List<(string Tweet, double Sentiment)>> GenerateActorTweetsAsync(string name, string gender, int age)
    {
        return await ActorTweetsParallel(name, gender, age);
    }

    private async Task<List<(string Review, double Sentiment)>> MovieReviewsParallel(string title, int year, string genre)
    {
        try
        {
            var chatClient = _openAIClient.GetChatClient(AiDeployment);
            var reviews = new List<(string Review, double Sentiment)>();
            var attempts = 0;
            const int maxAttempts = 20;
            const int batchSize = 4;

            string[] shuffledPersonas = personas.ToArray();
            var rng = new Random();
            rng.Shuffle(shuffledPersonas);
            int personaIndex = 0;

            var systemMessage = new SystemChatMessage(
                "You are a film critic providing a single concise review (2-3 sentences) " +
                "that begins directly with the rating, not with the movie title. " +
                "The review should focus on the film quality and style, not on individual actors. " +
                "Suggested aspects to cover include: "+
                "plot, screenwriting, symbolism, effects, or cinematography. " +
                "Your response must follow this format exactly:\n" +
                "Rating: X/10. [Your review text here]"
            );

            while (reviews.Count < NumCritics && attempts < maxAttempts)
            {
                var remainingReviews = NumCritics - reviews.Count;
                var currentBatchSize = Math.Min(batchSize, remainingReviews);
                
                var tasks = Enumerable.Range(0, currentBatchSize).Select(async i =>
                {
                    try
                    {
                        var persona = shuffledPersonas[(personaIndex + i) % shuffledPersonas.Length];
                        var messages = new ChatMessage[]
                        {
                            systemMessage,
                            new UserChatMessage(
                                $"{persona} Review the {genre} movie {title} ({year}). "                               
                            )
                        };

                        var response = await chatClient.CompleteChatAsync(messages);
                        var reviewText = response.Value.Content[0].Text.Trim();
                        
                        if (!reviewText.Contains("/10") || !reviewText.StartsWith("Rating:"))
                        {
                            return ("", 0.0);
                        }

                        var sentiment = _sentimentAnalyzer.PolarityScores(reviewText).Compound;
                        return (reviewText, sentiment);
                    }
                    catch (Exception)
                    {
                        return ("", 0.0);
                    }
                }).ToList();

                var results = await Task.WhenAll(tasks);
                reviews.AddRange(results.Where(r => !string.IsNullOrEmpty(r.Item1)));
                personaIndex = (personaIndex + currentBatchSize) % shuffledPersonas.Length;
                attempts++;

                if (reviews.Count < NumCritics && attempts < maxAttempts)
                {
                    await Task.Delay(1000);
                }
            }

            return reviews.Take(NumCritics).ToList();
        }
        catch (Exception ex)
        {
            throw new AIServiceException("Failed to generate movie reviews", ex);
        }
    }
        
    private (string Tweet, string Username) CleanTweetResponse(string response)
    {
        try
        {
            int atIndex = response.IndexOf('@');
            if ((atIndex == -1) || response.Contains("username"))
            {
                return ("", "");
            }
            string tweetText = response[..atIndex].Trim();
            tweetText = tweetText.Trim('"', '[', ']', ' ');
            
            string username = response[(atIndex + 1)..];
            int endUsername = username.IndexOfAny(new[] { ' ', ']'});
            if (endUsername != -1)
            {
                tweetText = $"{tweetText} {username[(endUsername + 1)..]}";
                username = username[..endUsername];
            }
            username = username.Trim('[', ']', '"', ' ');

            return (tweetText, username);
        }
        catch
        {
            return ("", "");
        }
    }

    private async Task<List<(string Tweet, double Sentiment)>> ActorTweetsParallel(string name, string gender, int age)
    {
        try
        {
            var chatClient = _openAIClient.GetChatClient(AiDeployment);
            var tweets = new List<(string Tweet, double Sentiment)>();
            var attempts = 0;
            const int maxAttempts = 30;
            const int batchSize = 4;

            string[] shuffledPersonas = personas.ToArray();
            var rng = new Random();
            rng.Shuffle(shuffledPersonas);
            int personaIndex = 0;

            var systemMessage = new SystemChatMessage(
                "You are a social media user providing a single authentic tweet (280 characters or less) " +
                "about an actor followed by your unique creative username. " +
                "Your tweet should sound like it's from a genuine fan or moviegoer. " +
                "Focus on any of these aspects: talent, memorable roles, personality, or public image. " +
                "Actor age is provided for identification purposes only. Do not use it in your tweet. " +
                "Your username should be imaginative, memorable, or playful, not a literal description of your persona. " +
                "Your response must follow this format exactly:\n" +
                "[Your tweet text here] @[Your username here]"
            );

            while(tweets.Count < NumTweets && attempts < maxAttempts)
            {
                var remainingTweets = NumTweets - tweets.Count;
                var currentBatchSize = Math.Min(batchSize, remainingTweets);

                var tasks = Enumerable.Range(0, currentBatchSize).Select(async i =>
                {
                    try
                    {
                        var persona = shuffledPersonas[(personaIndex + i) % shuffledPersonas.Length];
                        var messages = new ChatMessage[]
                        {
                            systemMessage,
                            new UserChatMessage(
                                $"{persona} Write a tweet about the {gender} actor {name}, who is {age} years old."
                            )
                        };

                        var response = await chatClient.CompleteChatAsync(messages);
                        var rawText = response.Value.Content[0].Text.Trim();
                        var (tweetText, username) = CleanTweetResponse(rawText);
                        var finalText = $"{tweetText} @{username}";

                        if (tweetText.Length > 280 || string.IsNullOrEmpty(tweetText) || string.IsNullOrEmpty(username))
                        {
                            return ("", 0.0);
                        }

                        var sentiment = _sentimentAnalyzer.PolarityScores(tweetText).Compound;
                        return (finalText, sentiment);
                    }
                    catch (Exception)
                    {
                        return ("", 0.0);
                    }
                }).ToList();

                var results = await Task.WhenAll(tasks);
                tweets.AddRange(results.Where(r => !string.IsNullOrEmpty(r.Item1)));
                personaIndex = (personaIndex + currentBatchSize) % shuffledPersonas.Length;
                attempts++;

                if (tweets.Count < NumTweets && attempts < maxAttempts)
                {
                    await Task.Delay(1000);
                }
            }

            return tweets.Take(NumTweets).ToList();
        }
        catch (Exception ex)
        {
            throw new AIServiceException("Failed to generate actor tweets", ex);
        }
    }
}
