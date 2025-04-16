using SSI.Trivia.Shared.Models.GenerationModels;

namespace SSI.Trivia.Shared.Interfaces;

public interface IOpenAITriviaService
{
    Task<TriviaGenerationResult> GenerateTrivia(string category, int questionCount, bool includeTieBreaker);
}