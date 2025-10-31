using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentQuizSystem.Data.Entities;
using StudentQuizSystem.Data.Identity;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StudentQuizSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>
        options) : base(options) { }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<StudentAttempt> StudentAttempts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Quiz>(b =>
            {
                b.HasKey(q => q.Id);
                b.HasMany(q => q.Questions).WithOne(qt =>
                qt.Quiz!).HasForeignKey(qt => qt.QuizId).OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<Question>(b =>
            {
                b.HasKey(q => q.Id);
                b.HasMany(q => q.Options).WithOne(o =>
                o.Question!).HasForeignKey(o => o.QuestionId).OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<Option>(b =>
            {
                b.HasKey(o => o.Id);
            });
            builder.Entity<StudentAttempt>(b =>
            {
                b.HasKey(a => a.Id);
            });
        }
    }
}