using StudentQuizSystem.Data.Models;

namespace StudentQuizSystem.Services.DTOs.Quiz
{
    public class QuizTakeViewModel
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();

        // This will hold the user's answers, mapping QuestionId to selected OptionId
        public Dictionary<int, int> Answers { get; set; } = new Dictionary<int, int>();
    }
}