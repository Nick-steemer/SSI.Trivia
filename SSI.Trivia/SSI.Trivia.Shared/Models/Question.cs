using System.ComponentModel.DataAnnotations;

namespace SSI.Trivia.Shared.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public bool IsTieBreaker { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public List<Answer> Answers { get; set; } = new();
        public Answer? TieBreakerAnswer { get; set; } // For tie-breaker comparison
        public string? MediaUrl { get; set; }
        public string? AfterMediaUrl { get; set; }

        public string? UploadedMediaBase64 { get; set; }
        public string? UploadedMediaName { get; set; }
        public string? UploadedAfterMediaBase64 { get; set; }
        public string? UploadedAfterMediaName { get; set; }
    }
}
