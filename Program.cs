using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CleanLife.Web.Data;
using CleanLife.Web.Models;
using CleanLife.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=cleanlife.db"));

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

builder.Services.AddScoped<IHabitService, HabitService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Habit}/{action=Index}/{id?}");

SeedData(app.Services);

app.Run();

static void SeedData(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.Migrate();

    if (context.Users.Any())
        return;

    var user = new User
    {
        UserName = "testuser",
        Email = "test@example.com"
    };

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

    using var transaction = context.Database.BeginTransaction();

    try
    {
        context.Users.Add(user);
        context.SaveChanges();

        habits[0].UserId = user.Id;
        habits[1].UserId = user.Id;

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