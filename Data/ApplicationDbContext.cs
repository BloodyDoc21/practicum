namespace CleanLife.Web.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CleanLife.Web.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Habit> Habits { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<HabitProgress> HabitProgresses { get; set; }
    public DbSet<HabitTag> HabitTags { get; set; }


    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
        }

        return base.SaveChanges();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Tag>()
            .HasIndex(t => new { t.Name, t.OwnerId })
            .IsUnique();

        modelBuilder.Entity<Habit>()
    .HasIndex(h => h.Status);

        modelBuilder.Entity<HabitProgress>()
            .HasIndex(p => p.Date);

        modelBuilder.Entity<Habit>()
            .HasOne(h => h.User)
            .WithMany(u => u.Habits)
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HabitProgress>()
            .HasOne(p => p.Habit)
            .WithMany(h => h.ProgressRecords)
            .HasForeignKey(p => p.HabitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}