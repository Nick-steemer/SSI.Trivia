using System.ComponentModel.DataAnnotations;

namespace SSI.Trivia.Shared.Models
{
    public class Sprint
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public List<Question> Questions { get; set; } = new();
        public bool IsComplete { get; set; }
    }
}
