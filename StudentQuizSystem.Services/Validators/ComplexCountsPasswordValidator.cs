using Microsoft.AspNetCore.Identity;
using StudentQuizSystem.Data.Identity;

namespace StudentQuizSystem.Services.Validators
{
    public class ComplexCountsPasswordValidator :
    IPasswordValidator<ApplicationUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser>
        manager, ApplicationUser user, string? password)
        {
            var errors = new List<IdentityError>();
            if (string.IsNullOrEmpty(password)) return
            Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Description =
            "Password cannot be empty"
            }));
            int uppercase = password.Count(char.IsUpper);
            int digits = password.Count(char.IsDigit);
            int symbols = password.Count(c => !char.IsLetterOrDigit(c));
            if (uppercase < 2)
                errors.Add(new IdentityError
                {
                    Description = "Password must contain at least 2 uppercase letters." });
if (digits < 3)
                errors.Add(new IdentityError
                {
                    Description = "Password must contain at least 3 digits." });
if (symbols < 3)
                errors.Add(new IdentityError
                {
                    Description = "Password must contain at least 3 symbols." });
if (errors.Any()) return
Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            return Task.FromResult(IdentityResult.Success);
        }
    }
}