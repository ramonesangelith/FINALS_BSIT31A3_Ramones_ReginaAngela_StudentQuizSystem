using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentQuizSystem.Data.Identity;
using StudentQuizSystem.Services;

namespace StudentQuizSystem.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly UserManager<ApplicationUser> _userManager;
        public QuizController(IQuizService quizService,
        UserManager<ApplicationUser> userManager)
        {
            _quizService = quizService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var quizzes = await _quizService.GetAllQuizzesAsync();
            return View(quizzes);
        }
        public async Task<IActionResult> Take(int id)
        {
            var quiz = await _quizService.GetQuizByIdAsync(id);
            if (quiz == null) return NotFound();
            return View(quiz);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int quizId, [FromForm]
List<int> selectedOptionIds)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();
            var attempt = await _quizService.SubmitAttemptAsync(user.Id,
            quizId, selectedOptionIds);
            return RedirectToAction("Result", new { id = attempt.Id });
        }
        public async Task<IActionResult> Result(int id)
        {
            // minimal: display attempt summary
            // For brevity we fetch attempt directly from DB
            var attempt = (await
            _quizService.GetLeaderboardAsync(0)).FirstOrDefault();
            return View();
        }
    }
}
