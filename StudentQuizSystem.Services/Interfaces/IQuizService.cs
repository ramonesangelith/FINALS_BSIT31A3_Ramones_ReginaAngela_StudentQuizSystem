using StudentQuizSystem.Data.Entities;

namespace StudentQuizSystem.Services
{
    public interface IQuizService
    {
        Task<List<Quiz>> GetAllQuizzesAsync();
        Task<Quiz?> GetQuizByIdAsync(int id);
        Task<Quiz> CreateQuizAsync(Quiz quiz);
        Task<StudentAttempt> SubmitAttemptAsync(string userId, int quizId, List<int> selectedOptionIds);
        Task<List<LeaderboardEntry>> GetLeaderboardAsync(int quizId, int top = 10);
    }
}
