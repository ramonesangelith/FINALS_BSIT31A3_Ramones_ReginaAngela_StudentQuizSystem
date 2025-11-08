using StudentQuizSystem.Services.DTOs.Home; // We will create this ViewModel soon

namespace StudentQuizSystem.Services
{
    public interface ILeaderboardService
    {
        Task<IEnumerable<LeaderboardViewModel>> GetLeaderboardAsync(string? subject = null);
    }
}