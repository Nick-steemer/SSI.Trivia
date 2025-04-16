using System.ComponentModel.DataAnnotations;

namespace SSI.Trivia.Shared.Models;

public class Participant
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    // Navigation properties
    public List<ParticipantSprint> Sprints { get; set; } = new();
    public List<ParticipantAnswer> Answers { get; set; } = new();
}
