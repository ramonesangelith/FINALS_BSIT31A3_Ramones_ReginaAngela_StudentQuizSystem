namespace StudentQuizSystem.Services.DTOs.Home
{
    public class HomeViewModel
    {
        public IEnumerable<StudentQuizSystem.Data.Models.Quiz> Quizzes { get; set; } = new List<StudentQuizSystem.Data.Models.Quiz>();
    }
}