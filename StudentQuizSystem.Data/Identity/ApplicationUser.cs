using Microsoft.AspNetCore.Identity;

namespace StudentQuizSystem.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Additional profile fields (optional)
        public string? FullName { get; set; }
    }
}
