using StudentQuizSystem.Data.Models;
using StudentQuizSystem.Services.DTOs.Quiz;

namespace StudentQuizSystem.Services
{
    public interface IQuizService
    {
        Task<IEnumerable<Quiz>> GetAllQuizzesAsync();
        Task<Quiz?> GetQuizForTakingAsync(int quizId);
        Task CreateQuizAsync(QuizCreateViewModel model);
        Task DeleteQuizAsync(int quizId);
        Task<int> CalculateScoreAsync(int quizId, Dictionary<int, int> answers);
        Task SaveAttemptAsync(int quizId, string studentId, int score);
    }
}