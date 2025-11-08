using Microsoft.EntityFrameworkCore;
using StudentQuizSystem.Data;
using StudentQuizSystem.Services.DTOs.Home;

namespace StudentQuizSystem.Services.Implementation
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly ApplicationDbContext _context;

        public LeaderboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaderboardViewModel>> GetLeaderboardAsync(string? subject = null)
        {
            var query = _context.QuizAttempts
                .Include(qa => qa.Student)
                .Include(qa => qa.Quiz)
                .AsQueryable();

            if (!string.IsNullOrEmpty(subject))
            {
                query = query.Where(qa => qa.Quiz.Subject == subject);
            }

            // Get the highest score for each student per subject
            var highScores = await query
                .GroupBy(qa => new { qa.StudentId, qa.Quiz.Subject })
                .Select(g => new
                {
                    StudentName = g.First().Student.UserName, // or FullName if you added it
                    Subject = g.Key.Subject,
                    MaxScore = g.Max(qa => qa.Score)
                })
                .OrderByDescending(x => x.MaxScore)
                .ThenBy(x => x.StudentName)
                .ToListAsync();

            // Re-shape into ViewModel
            return highScores.Select(hs => new LeaderboardViewModel
            {
                StudentName = hs.StudentName ?? "Unknown",
                Subject = hs.Subject,
                HighestScore = hs.MaxScore
            });
        }
    }
}