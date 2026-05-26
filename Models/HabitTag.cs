using CleanLife.Web.Models;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(HabitId), nameof(TagId))]
public class HabitTag
{
    public int HabitId { get; set; }
    public int TagId { get; set; }

    public Habit? Habit { get; set; }
    public Tag? Tag { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}