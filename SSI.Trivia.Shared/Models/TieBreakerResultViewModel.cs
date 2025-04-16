namespace SSI.Trivia.Shared.Models;

public class TieBreakerResultViewModel
{
    public int ParticipantId { get; set; }
    public string ParticipantName { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
}
