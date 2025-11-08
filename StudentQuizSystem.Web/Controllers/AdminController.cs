using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentQuizSystem.Services;
using StudentQuizSystem.Services.DTOs.Quiz;

namespace StudentQuizSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IQuizService _quizService;

        public AdminController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        public IActionResult Index()
        {
            return View(); // A dashboard for admin
        }

        [HttpGet]
        public IActionResult CreateQuiz()
        {
            // Initialize with one question and 4 options
            var model = new QuizCreateViewModel();
            model.Questions.Add(new QuestionViewModel
            {
                Options = new List<OptionViewModel> { new(), new(), new(), new() }
            });
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuiz(QuizCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _quizService.CreateQuizAsync(model);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            await _quizService.DeleteQuizAsync(id);
            return RedirectToAction("Index", "Home");
        }
    }
}