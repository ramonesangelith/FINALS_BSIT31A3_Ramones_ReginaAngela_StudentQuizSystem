using System.ComponentModel.DataAnnotations;

namespace StudentQuizSystem.Services.DTOs.Quiz
{
    public class QuizCreateViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Subject { get; set; }

        // This allows dynamic adding of questions in the view
        public List<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
    }

    public class QuestionViewModel
    {
        [Required]
        public string Text { get; set; }
        public List<OptionViewModel> Options { get; set; } = new List<OptionViewModel>();
    }

    public class OptionViewModel
    {
        [Required]
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}