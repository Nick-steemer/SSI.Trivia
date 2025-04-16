namespace SSI.Trivia.Shared.Models.GenerationModels;

public class TriviaGenerationResult
{
    public List<TriviaQuestionItem> Questions { get; set; } = new();
    public TieBreakerItem TieBreaker { get; set; }
    public string Error { get; set; }
}
