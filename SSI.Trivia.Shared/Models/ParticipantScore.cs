namespace SSI.Trivia.Shared.Models;

public class ParticipantScore
{
    public int ParticipantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Score { get; set; }
    public int Rank { get; set; }
    public int CorrectAnswers { get; set; }
    public int TotalQuestions { get; set; }
}