using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentQuizSystem.Data;
using StudentQuizSystem.Data.Identity;
using StudentQuizSystem.Services;
using StudentQuizSystem.Services.Validators;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
// Use EF Core InMemory for the required EF Core in-memory implementation
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseInMemoryDatabase("StudentQuizDb"));
// Identity with custom options and custom password validator
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false; // We'll enforce manually in validator
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false; // count enforced by custom validator
    options.Password.RequireNonAlphanumeric = false; // count enforced by custom validator
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
// Register the custom password validator
builder.Services.AddTransient<IPasswordValidator<ApplicationUser>,
ComplexCountsPasswordValidator>();
// Register application services (service layer)
builder.Services.AddScoped<IQuizService, QuizService>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");
// Seed sample data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DataSeed.Seed(db);
}
app.Run();
