namespace StudentQuizSystem.Data.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}