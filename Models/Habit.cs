using System.ComponentModel.DataAnnotations;

namespace CleanLife.Web.Models
{
    public class Habit

    {

        [Range(1, 3650, ErrorMessage = "Цель должна быть больше 0")]
        public int GoalDays { get; set; }
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        [Required]
        

      
        public string Status { get; set; } = "Active"; // Active, Completed, Failed

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<HabitProgress> ProgressRecords { get; set; } = new List<HabitProgress>();
        public ICollection<HabitTag> HabitTags { get; set; } = new List<HabitTag>();

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
