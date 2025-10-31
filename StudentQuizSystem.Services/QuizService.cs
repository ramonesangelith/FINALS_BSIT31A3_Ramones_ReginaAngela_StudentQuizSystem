using Microsoft.EntityFrameworkCore;
using StudentQuizSystem.Data;
using StudentQuizSystem.Data.Entities;

namespace StudentQuizSystem.Services
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext _db;
        public QuizService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Quiz> CreateQuizAsync(Quiz quiz)
        {
            _db.Quizzes.Add(quiz);
            await _db.SaveChangesAsync();
            return quiz;
        }
        public async Task<List<Quiz>> GetAllQuizzesAsync()
        {
            return await _db.Quizzes.Include(q => q.Questions!).ThenInclude(qt
            => qt.Options!).ToListAsync();
        }
        public async Task<Quiz?> GetQuizByIdAsync(int id)
        {
            return await _db.Quizzes.Include(q => q.Questions!).ThenInclude(qt
            => qt.Options!).FirstOrDefaultAsync(q => q.Id == id);
        }
        public async Task<StudentAttempt> SubmitAttemptAsync(string userId, int
        quizId, List<int> selectedOptionIds)
        {
            var quiz = await GetQuizByIdAsync(quizId);
            if (quiz == null) throw new ArgumentException("Quiz not found");
            // compute score: +1 per question with correct selected option (supports single - correct)
var questions = quiz.Questions ?? new List<Question>();
            int score = 0;
            foreach (var q in questions)
            {
                var correctOption = q.Options?.FirstOrDefault(o => o.IsCorrect);
                if (correctOption != null &&
                selectedOptionIds.Contains(correctOption.Id))
                {
                    score++;
                }
            }
            var attempt = new StudentAttempt
            {
                UserId = userId,
                QuizId = quizId,
                Score = score,
                SelectedOptionIds = selectedOptionIds
            };
            _db.StudentAttempts.Add(attempt);
            await _db.SaveChangesAsync();
            return attempt;
        }
        public async Task<List<LeaderboardEntry>> GetLeaderboardAsync(int quizId, int top = 10)
        {
            return await _db.StudentAttempts
                .Where(a => a.QuizId == quizId)
                .OrderByDescending(a => a.Score)
                .ThenBy(a => a.TakenAt)
                .Take(top)
                .Select(a => new LeaderboardEntry
                {
                    UserId = a.UserId,
                    Score = a.Score
                })
                .ToListAsync();
        }
    }
}
