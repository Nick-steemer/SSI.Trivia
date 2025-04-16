using System.ComponentModel.DataAnnotations;

namespace SSI.Trivia.Shared.Models
{
    public class Answer
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;
    }
}
