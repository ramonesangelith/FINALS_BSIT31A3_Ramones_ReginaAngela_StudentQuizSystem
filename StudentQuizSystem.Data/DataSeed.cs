using StudentQuizSystem.Data.Entities;

namespace StudentQuizSystem.Data
{
    public static class DataSeed
    {
        public static void Seed(ApplicationDbContext db)
        {
            if (db.Quizzes.Any()) return;
            var quiz = new Quiz
            {
                Title = "Intro to C#",
                Description = "Simple multiple choice quiz",
                Questions = new List<Question>
{
new Question
{
Text = "What is the keyword to define a class in C#?",
Options = new List<Option>
{
new Option{ Text = "class", IsCorrect = true },
new Option{ Text = "struct", IsCorrect = false },
new Option{ Text = "module", IsCorrect = false },
new Option{ Text = "def", IsCorrect = false }
}
},
new Question
{
Text = "Which of these is a value type?",
Options = new List<Option>
{
new Option{ Text = "int", IsCorrect = true },
new Option{ Text = "string", IsCorrect = false },
new Option{ Text = "Task", IsCorrect = false },
new Option{ Text = "IEnumerable", IsCorrect =
false }
}
}
}
            };
            db.Quizzes.Add(quiz);
            db.SaveChanges();
        }
    }
}