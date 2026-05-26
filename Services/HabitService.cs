using CleanLife.Web.Data;
using CleanLife.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanLife.Web.Services;

public class HabitService : IHabitService
{
    private readonly ApplicationDbContext _context;

    public HabitService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Habit>> GetUserHabitsAsync(string userId)
    {
        return await _context.Habits
            .Include(h => h.HabitTags)
            .ThenInclude(ht => ht.Tag)
            .Where(h => h.UserId == userId)
            .OrderByDescending(h => h.StartDate)
            .ToListAsync();
    }

    public async Task<Habit?> GetHabitByIdAsync(int id, string userId)
    {
        return await _context.Habits
            .Include(h => h.HabitTags)
            .ThenInclude(ht => ht.Tag)
            .FirstOrDefaultAsync(
                h => h.Id == id &&
                     h.UserId == userId);
    }

    public async Task<Habit> CreateHabitAsync(
        Habit habit,
        string userId,
        int[] selectedTags)
    {
        habit.UserId = userId;

        _context.Habits.Add(habit);

        await _context.SaveChangesAsync();

        if (selectedTags != null &&
            selectedTags.Any())
        {
            foreach (var tagId in selectedTags)
            {
                _context.HabitTags.Add(
                    new HabitTag
                    {
                        HabitId = habit.Id,
                        TagId = tagId
                    });
            }

            await _context.SaveChangesAsync();
        }

        return habit;
    }

    public async Task<Habit?> UpdateHabitAsync(
        Habit habit,
        string userId,
        int[] selectedTags)
    {
        var existingHabit =
            await _context.Habits
            .Include(h => h.HabitTags)
            .FirstOrDefaultAsync(
                h => h.Id == habit.Id &&
                     h.UserId == userId);

        if (existingHabit == null)
            return null;

        existingHabit.Name = habit.Name;
        existingHabit.Description = habit.Description;
        existingHabit.GoalDays = habit.GoalDays;
        existingHabit.Status = habit.Status;

        existingHabit.HabitTags.Clear();

        if (selectedTags != null &&
            selectedTags.Any())
        {
            foreach (var tagId in selectedTags)
            {
                existingHabit.HabitTags.Add(
                    new HabitTag
                    {
                        HabitId = habit.Id,
                        TagId = tagId
                    });
            }
        }

        await _context.SaveChangesAsync();

        return existingHabit;
    }

    public async Task<bool> DeleteHabitAsync(
        int id,
        string userId)
    {
        var habit =
            await _context.Habits
            .FirstOrDefaultAsync(
                h => h.Id == id &&
                     h.UserId == userId);

        if (habit == null)
            return false;

        _context.Habits.Remove(habit);

        await _context.SaveChangesAsync();

        return true;
    }
}