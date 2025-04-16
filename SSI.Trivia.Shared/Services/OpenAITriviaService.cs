using Microsoft.Extensions.Options;
using SSI.Trivia.Shared.Configuration;
using SSI.Trivia.Shared.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SSI.Trivia.Shared.Services;

public class OpenAITriviaService : IOpenAITriviaService
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://api.openai.com/v1/chat/completions";

    public OpenAITriviaService(HttpClient httpClient, IOptions<OpenAISettings> options)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Value.ApiKey);
    }

    public async Task<TriviaGenerationResult> GenerateTrivia(string category, int questionCount, bool includeTieBreaker)
    {
        var prompt = CreateTriviaPrompt(category, questionCount, includeTieBreaker);

        var requestBody = new
        {
            model = "gpt-4o",
            messages = new[]
            {
                new { role = "system", content = "You are a trivia question generator specializing in creating engaging, well-researched questions with multiple-choice answers." },
                new { role = "user", content = prompt }
            },
            temperature = 0.7
        };

        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(ApiUrl, content);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<OpenAIResponse>(responseBody);

        // Parse the response to extract questions and answers
        return ParseTriviaResponse(responseObject?.choices[0].message.content);
    }

    private string CreateTriviaPrompt(string category, int questionCount, bool includeTieBreaker)
    {
        var prompt = $@"Generate {questionCount} engaging trivia questions about {category} with 4 multiple-choice answers each. 
For each question, clearly indicate which answer is correct.

";

        if (includeTieBreaker)
        {
            prompt += @"Also include one tie-breaker question in the style of 'The Price is Right' where the answer is a specific number or amount. 
For this tie-breaker, provide the exact numerical answer.

";
        }

        prompt += @"Format your response as JSON with the following structure:
{
  ""questions"": [
    {
      ""questionText"": ""Question text here?"",
      ""answers"": [
        { ""text"": ""Answer 1"", ""isCorrect"": false },
        { ""text"": ""Answer 2"", ""isCorrect"": true },
        { ""text"": ""Answer 3"", ""isCorrect"": false },
        { ""text"": ""Answer 4"", ""isCorrect"": false }
      ]
    }
  ],
  ""tieBreaker"": {
    ""questionText"": ""How many X are there in Y?"",
    ""answer"": ""42""
  }
}";

        return prompt;
    }

    private TriviaGenerationResult ParseTriviaResponse(string response)
    {
        try
        {
            // Extract JSON from the response (in case there's any surrounding text)
            int startIndex = response.IndexOf('{');
            int endIndex = response.LastIndexOf('}') + 1;

            if (startIndex >= 0 && endIndex > startIndex)
            {
                string jsonText = response.Substring(startIndex, endIndex - startIndex);
                return JsonSerializer.Deserialize<TriviaGenerationResult>(jsonText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return new TriviaGenerationResult
            {
                Error = "Could not parse response as JSON"
            };
        }
        catch (Exception ex)
        {
            return new TriviaGenerationResult
            {
                Error = $"Error parsing response: {ex.Message}"
            };
        }
    }
}

public class OpenAIResponse
{
    public Choice[] choices { get; set; }

    public class Choice
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public string content { get; set; }
    }
}

public class TriviaGenerationResult
{
    public List<TriviaQuestionItem> Questions { get; set; } = new();
    public TieBreakerItem TieBreaker { get; set; }
    public string Error { get; set; }
}

public class TriviaQuestionItem
{
    public string QuestionText { get; set; }
    public List<TriviaAnswerItem> Answers { get; set; } = new();
}

public class TriviaAnswerItem
{
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
}

public class TieBreakerItem
{
    public string QuestionText { get; set; }
    public string Answer { get; set; }
}