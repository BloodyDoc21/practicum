using CleanLife.Web.Data;
using CleanLife.Web.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

    

SeedData(app.Services);

app.Run();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

static void SeedData(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.Migrate();

    if (context.Users.Any())
        return;

    var user = new User
    {
        Username = "testuser",
        Email = "test@example.com",
        PasswordHash = "hash"
    };

    context.Users.Add(user);
    context.SaveChanges();

    var habits = new[]
    {
        new Habit { Name = "Не курить", GoalDays = 30, UserId = user.Id },
        new Habit { Name = "Бегать", GoalDays = 14, UserId = user.Id }
    };

    context.Habits.AddRange(habits);
    context.SaveChanges();

    var progress = new[]
    {
        new HabitProgress { HabitId = habits[0].Id, Date = DateTime.UtcNow, IsCompleted = true },
        new HabitProgress { HabitId = habits[1].Id, Date = DateTime.UtcNow, IsCompleted = false }
    };

    using var transaction = context.Database.BeginTransaction();

    try
    {
        context.Users.Add(user);
        context.SaveChanges();

        context.Habits.AddRange(habits);
        context.SaveChanges();

        context.HabitProgresses.AddRange(progress);
        context.SaveChanges();

        transaction.Commit();
    }
    catch
    {
        transaction.Rollback();
        throw;
    }

    context.HabitProgresses.AddRange(progress);
    context.SaveChanges();
}




