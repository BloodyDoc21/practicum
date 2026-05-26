using CleanLife.Web.Models;

namespace CleanLife.Web.Services;

public interface IHabitService
{
    Task<IEnumerable<Habit>> GetUserHabitsAsync(string userId);

    Task<Habit?> GetHabitByIdAsync(int id, string userId);

    Task<Habit> CreateHabitAsync(
        Habit habit,
        string userId,
        int[] selectedTags);

    Task<Habit?> UpdateHabitAsync(
        Habit habit,
        string userId,
        int[] selectedTags);

    Task<bool> DeleteHabitAsync(
        int id,
        string userId);
}