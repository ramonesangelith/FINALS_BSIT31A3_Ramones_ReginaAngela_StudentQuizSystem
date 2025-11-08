using Microsoft.AspNetCore.Mvc;
using StudentQuizSystem.Services;
using StudentQuizSystem.Services.DTOs.Home;

namespace StudentQuizSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly ILeaderboardService _leaderboardService;

        public HomeController(IQuizService quizService, ILeaderboardService leaderboardService)
        {
            _quizService = quizService;
            _leaderboardService = leaderboardService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                Quizzes = await _quizService.GetAllQuizzesAsync()
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Leaderboard(string? subject)
        {
            ViewBag.Subject = subject;
            var leaderboard = await _leaderboardService.GetLeaderboardAsync(subject);
            return View(leaderboard);
        }
    }
}