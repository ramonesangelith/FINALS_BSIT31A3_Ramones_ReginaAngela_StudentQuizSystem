using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentQuizSystem.Data.Models;
using StudentQuizSystem.Services;

namespace StudentQuizSystem.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(IQuizService quizService, UserManager<ApplicationUser> userManager)
        {
            _quizService = quizService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> TakeQuiz(int id)
        {
            var quiz = await _quizService.GetQuizForTakingAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            var model = new StudentQuizSystem.Services.DTOs.Quiz.QuizTakeViewModel
            {
                QuizId = quiz.Id,
                Title = quiz.Title,
                Questions = quiz.Questions.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuiz(StudentQuizSystem.Services.DTOs.Quiz.QuizTakeViewModel model)
        {
            var studentId = _userManager.GetUserId(User);
            var score = await _quizService.CalculateScoreAsync(model.QuizId, model.Answers);

            await _quizService.SaveAttemptAsync(model.QuizId, studentId, score);

            var quiz = await _quizService.GetQuizForTakingAsync(model.QuizId);

            var resultModel = new StudentQuizSystem.Services.DTOs.Quiz.QuizResultViewModel
            {
                QuizTitle = quiz.Title,
                Score = score,
                TotalQuestions = quiz.Questions.Count
            };

            return View("Result", resultModel);
        }
    }
}