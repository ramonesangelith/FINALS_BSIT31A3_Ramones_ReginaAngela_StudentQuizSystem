using Microsoft.AspNetCore.Identity;

namespace StudentQuizSystem.Data.Models
{
    // Custom user class to extend IdentityUser
    public class ApplicationUser : IdentityUser
    {
        // You can add custom properties here if needed, e.g., FullName
        public string? FullName { get; set; }
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    }
}