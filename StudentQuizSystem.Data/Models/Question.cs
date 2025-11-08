namespace StudentQuizSystem.Data.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<Option> Options { get; set; } = new List<Option>();
    }
}