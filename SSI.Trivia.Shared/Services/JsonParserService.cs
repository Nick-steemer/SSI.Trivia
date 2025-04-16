using SSI.Trivia.Shared.Models.GenerationModels;
using System.Text.Json;

namespace SSI.Trivia.Shared.Services;

public static class JsonParserService
{
    public static TriviaGenerationResult ParseTriviaJson(string jsonInput)
    {
        try
        {
            // Extract JSON from the response (in case there's any surrounding text)
            int startIndex = jsonInput.IndexOf('{');
            int endIndex = jsonInput.LastIndexOf('}') + 1;

            if (startIndex < 0 || endIndex <= startIndex)
            {
                return new TriviaGenerationResult
                {
                    Error = "Could not find valid JSON in the input"
                };
            }

            string jsonText = jsonInput.Substring(startIndex, endIndex - startIndex);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<TriviaGenerationResult>(jsonText, options);

            // Validate the result
            if (result?.Questions == null || result.Questions.Count == 0)
            {
                return new TriviaGenerationResult
                {
                    Error = "No questions found in JSON data"
                };
            }

            return result;
        }
        catch (JsonException jsonEx)
        {
            return new TriviaGenerationResult
            {
                Error = $"JSON parsing error: {jsonEx.Message}"
            };
        }
        catch (Exception ex)
        {
            return new TriviaGenerationResult
            {
                Error = $"Error parsing trivia JSON: {ex.Message}"
            };
        }
    }
}
