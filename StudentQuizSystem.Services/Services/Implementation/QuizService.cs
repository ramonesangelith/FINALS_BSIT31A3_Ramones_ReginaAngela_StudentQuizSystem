using Microsoft.EntityFrameworkCore;
using StudentQuizSystem.Data;
using StudentQuizSystem.Data.Models;
using StudentQuizSystem.Services.DTOs.Quiz;

namespace StudentQuizSystem.Services.Implementation
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext _context;

        public QuizService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Quiz>> GetAllQuizzesAsync()
        {
            return await _context.Quizzes.AsNoTracking().ToListAsync();
        }

        public async Task<Quiz?> GetQuizForTakingAsync(int quizId)
        {
            return await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Options)
                .AsNoTracking()
                .FirstOrDefaultAsync(q => q.Id == quizId);
        }

        public async Task CreateQuizAsync(QuizCreateViewModel model)
        {
            var quiz = new Quiz
            {
                Title = model.Title,
                Subject = model.Subject,
                Questions = model.Questions.Select(q => new Question
                {
                    Text = q.Text,
                    Options = q.Options.Select(o => new Option
                    {
                        Text = o.Text,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                }).ToList()
            };

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuizAsync(int quizId)
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if (quiz != null)
            {
                // Due to cascade delete, questions and options will be removed.
                // We need to manually remove attempts.
                var attempts = _context.QuizAttempts.Where(qa => qa.QuizId == quizId);
                _context.QuizAttempts.RemoveRange(attempts);

                _context.Quizzes.Remove(quiz);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CalculateScoreAsync(int quizId, Dictionary<int, int> answers)
        {
            var questions = await _context.Questions
                .Where(q => q.QuizId == quizId)
                .Include(q => q.Options)
                .ToListAsync();

            int score = 0;
            foreach (var question in questions)
            {
                int correctOptionId = question.Options.First(o => o.IsCorrect).Id;
                if (answers.ContainsKey(question.Id) && answers[question.Id] == correctOptionId)
                {
                    score++;
                }
            }
            return score;
        }

        public async Task SaveAttemptAsync(int quizId, string studentId, int score)
        {
            var attempt = new QuizAttempt
            {
                QuizId = quizId,
                StudentId = studentId,
                Score = score,
                DateTaken = DateTime.UtcNow
            };
            _context.QuizAttempts.Add(attempt);
            await _context.SaveChangesAsync();
        }
    }
}