namespace SSI.Trivia.Shared.Models.GenerationModels;

public class TriviaQuestionItem
{
    public string QuestionText { get; set; }
    public List<TriviaAnswerItem> Answers { get; set; } = new();
}
