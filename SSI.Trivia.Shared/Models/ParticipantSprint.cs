using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSI.Trivia.Shared.Models;

public class ParticipantSprint
{
    [Key]
    public int Id { get; set; }
    
    public int ParticipantId { get; set; }
    [ForeignKey("ParticipantId")]
    public Participant Participant { get; set; } = null!;
    
    public int SprintId { get; set; }
    [ForeignKey("SprintId")]
    public Sprint Sprint { get; set; } = null!;
    
    // Calculated score for this participant in this sprint
    public int Score { get; set; }
}
