using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentQuizSystem.Data.Entities;
using StudentQuizSystem.Services;

namespace StudentQuizSystem.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IQuizService _quizService;
        public AdminController(IQuizService quizService)
        {
            _quizService = quizService;
        }
        // GET: Admin/Create
        public IActionResult Create() => View();
        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Quiz quiz)
        {
            if (!ModelState.IsValid) return View(quiz);
            await _quizService.CreateQuizAsync(quiz);
            return RedirectToAction("Index", "Home");
        }
    }
}
