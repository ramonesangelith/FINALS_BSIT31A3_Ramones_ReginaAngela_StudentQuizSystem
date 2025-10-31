using System.ComponentModel.DataAnnotations;

namespace StudentQuizSystem.Data.Entities
{
    public class Question
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        public ICollection<Option>? Options { get; set; }
    }
}