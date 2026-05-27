using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CleanLife.Web.Data;
using CleanLife.Web.Models;
using CleanLife.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// MVC + API
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=cleanlife.db"));

// Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Services
builder.Services.AddScoped<IHabitService, HabitService>();

// Cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

var app = builder.Build();

// Middleware
app.UseStaticFiles();

app.UseRouting();

app.UseCors("ReactPolicy");

app.UseAuthentication();

app.UseAuthorization();

// API controllers
app.MapControllers();

// MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Habit}/{action=Index}/{id?}");

// Seed
SeedData(app.Services);

app.Run();

static void SeedData(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();

    var context =
        scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    context.Database.Migrate();

    if (context.Users.Any())
        return;

    var user = new User
    {
        UserName = "testuser",
        Email = "test@example.com"
    };

    using var transaction =
        context.Database.BeginTransaction();

    try
    {
        context.Users.Add(user);

        context.SaveChanges();

        var habits = new[]
        {
            new Habit
            {
                Name = "Не курить",
                GoalDays = 30,
                UserId = user.Id,
                Status = "Active"
            },

            new Habit
            {
                Name = "Бегать",
                GoalDays = 14,
                UserId = user.Id,
                Status = "Active"
            }
        };

        context.Habits.AddRange(habits);

        context.SaveChanges();

        var progress = new[]
        {
            new HabitProgress
            {
                HabitId = habits[0].Id,
                Date = DateTime.UtcNow,
                IsCompleted = true
            },

            new HabitProgress
            {
                HabitId = habits[1].Id,
                Date = DateTime.UtcNow,
                IsCompleted = false
            }
        };

        context.HabitProgresses.AddRange(progress);

        context.SaveChanges();

        transaction.Commit();
    }
    catch
    {
        transaction.Rollback();

        throw;
    }
}