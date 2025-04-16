using SSI.Trivia.Shared.Services;

namespace SSI.Trivia.Shared.Interfaces;

public interface IOpenAITriviaService
{
    Task<TriviaGenerationResult> GenerateTrivia(string category, int questionCount, bool includeTieBreaker);
}