using System.ComponentModel.DataAnnotations;

namespace SSI.Trivia.Shared.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public bool IsTieBreaker { get; set; }
        [MaxLength(500)]
        public string QuestionText { get; set; } = string.Empty;
        public List<Answer> Answers { get; set; } = new();
        public Answer? TieBreakerAnswer { get; set; } // For tie-breaker comparison
        [MaxLength(5000)]
        public string? MediaUrl { get; set; }
        [MaxLength(5000)]
        public string? AfterMediaUrl { get; set; }

        [MaxLength(5000)]
        public string? UploadedMediaBase64 { get; set; }
        [MaxLength(5000)]
        public string? UploadedMediaName { get; set; }
        [MaxLength(5000)]
        public string? UploadedAfterMediaBase64 { get; set; }
        [MaxLength(5000)]
        public string? UploadedAfterMediaName { get; set; }

        public Guid? CorrectAnswerId { get; set; }
    }
}
