using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSI.Trivia.Shared.Models;

public class ParticipantAnswer
{
    [Key]
    public int Id { get; set; }
    
    public int ParticipantId { get; set; }
    [ForeignKey("ParticipantId")]
    public Participant Participant { get; set; } = null!;
    
    public int QuestionId { get; set; }
    [ForeignKey("QuestionId")]
    public Question Question { get; set; } = null!;
    
    // For multiple choice questions
    public Guid? SelectedAnswerId { get; set; }
    [ForeignKey("SelectedAnswerId")]
    public Answer? SelectedAnswer { get; set; }
    
    // For tie-breaker questions
    [MaxLength(500)]
    public string? TieBreakerText { get; set; }
}
