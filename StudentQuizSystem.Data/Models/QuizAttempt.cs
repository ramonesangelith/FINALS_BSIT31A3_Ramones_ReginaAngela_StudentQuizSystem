namespace StudentQuizSystem.Data.Models
{
    public class QuizAttempt
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime DateTaken { get; set; }

        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }

        public string StudentId { get; set; }
        public virtual ApplicationUser Student { get; set; }
    }
}