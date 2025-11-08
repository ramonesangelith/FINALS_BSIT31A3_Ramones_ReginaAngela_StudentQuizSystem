using Microsoft.AspNetCore.Identity;
using StudentQuizSystem.Data.Models;
using System.Text.RegularExpressions;

namespace StudentQuizSystem.Web.Identity
{
    public class CustomPasswordValidator : IPasswordValidator<ApplicationUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string? password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "Password cannot be null or empty." }));
            }

            var errors = new List<IdentityError>();

            // Requirement: At least 2 uppercase letters
            if (password.Count(char.IsUpper) < 2)
            {
                errors.Add(new IdentityError { Description = "Passwords must have at least two uppercase letters ('A'-'Z')." });
            }

            // Requirement: At least 3 numbers
            if (password.Count(char.IsDigit) < 3)
            {
                errors.Add(new IdentityError { Description = "Passwords must have at least three digits ('0'-'9')." });
            }

            // Requirement: At least 3 symbols
            var symbolCount = Regex.Matches(password, @"[!@#$%^&*(),.?""{}|<>]").Count;
            if (symbolCount < 3)
            {
                errors.Add(new IdentityError { Description = "Passwords must have at least three non-alphanumeric symbols." });
            }

            // You can add a length requirement here too
            if (password.Length < 10)
            {
                errors.Add(new IdentityError { Description = "Passwords must be at least 10 characters long." });
            }

            return Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}