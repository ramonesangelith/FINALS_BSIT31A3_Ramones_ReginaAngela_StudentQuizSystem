using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentQuizSystem.Data;
using StudentQuizSystem.Data.Models;
using StudentQuizSystem.Services;
using StudentQuizSystem.Services.Implementation;
using StudentQuizSystem.Web.Identity; // Import the custom validator

var builder = WebApplication.CreateBuilder(args);

// 1. Add DbContext and In-Memory Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("StudentQuizDb")); // As requested: In-Memory DB

// 2. Add ASP.NET Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 3. REMOVE the default password validator and ADD our custom one
builder.Services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordValidator>();

// 4. Configure Identity options (we use the custom validator, but can set other rules)
builder.Services.Configure<IdentityOptions>(options =>
{
    // We set basic rules here; our CustomPasswordValidator enforces the complex ones.
    options.Password.RequireDigit = false; // Handled by custom validator
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false; // Handled by custom validator
    options.Password.RequireNonAlphanumeric = false; // Handled by custom validator
    options.Password.RequiredLength = 10;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;

    options.User.RequireUniqueEmail = true;
});

// 5. Add application services
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();

// 6. Add MVC Controllers and Views
builder.Services.AddControllersWithViews();

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

app.UseAuthentication(); // Must be before UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// --- Data Seeding ---
// We seed the database with Admin/Student roles and a default Admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedRolesAndAdminUser(userManager, roleManager);
}
// --- End Seeding ---

app.Run();


// Seeding Method
async Task SeedRolesAndAdminUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    // Seed Roles
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }
    if (!await roleManager.RoleExistsAsync("Student"))
    {
        await roleManager.CreateAsync(new IdentityRole("Student"));
    }

    // Seed Admin User
    var adminEmail = "admin@quiz.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        // Password must match our custom rules!
        // "AdminPass!@#123" -> 2 Upper (A,P), 3 Symbols (!@#), 3 Numbers (123)
        var result = await userManager.CreateAsync(adminUser, "AdminPass!@#123");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}