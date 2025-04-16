namespace SSI.Trivia.Shared.Models;

public class TieBreakerResult
{
    public int ParticipantId { get; set; }
    public string Answer { get; set; } = string.Empty;
    public int NumericValue { get; set; }
    public int Difference { get; set; } // Difference from target value
}
