using System.ComponentModel.DataAnnotations;

namespace StudentQuizSystem.Data.Entities
{
    public class Quiz
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Question>? Questions { get; set; }
    }
}