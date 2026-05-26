namespace CleanLife.Web.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public required string Text { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int HabitId { get; set; }

        public Habit? Habit { get; set; }

        public string UserId { get; set; }

        public User? User { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}   