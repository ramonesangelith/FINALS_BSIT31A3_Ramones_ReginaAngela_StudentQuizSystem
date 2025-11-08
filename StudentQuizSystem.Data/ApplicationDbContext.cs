using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentQuizSystem.Data.Models;

namespace StudentQuizSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<Quiz>()
                .HasMany(q => q.Questions)
                .WithOne(q => q.Quiz)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting a quiz deletes its questions

            builder.Entity<Question>()
                .HasMany(q => q.Options)
                .WithOne(o => o.Question)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting a question deletes its options

            builder.Entity<QuizAttempt>()
               .HasOne(qa => qa.Student)
               .WithMany(u => u.QuizAttempts)
               .HasForeignKey(qa => qa.StudentId);

            builder.Entity<QuizAttempt>()
               .HasOne(qa => qa.Quiz)
               .WithMany() // No navigation property back from Quiz if not needed
               .HasForeignKey(qa => qa.QuizId);
        }
    }
}