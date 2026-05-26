namespace CleanLife.Web.Models
{
    public class HabitProgress
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public bool IsCompleted { get; set; }

        public int HabitId { get; set; }
        public Habit? Habit { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
