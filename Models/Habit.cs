using System.ComponentModel.DataAnnotations;

namespace CleanLife.Web.Models
{
    public class Habit
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Цель (дней)")]
        [Range(1, 3650, ErrorMessage = "Цель должна быть больше 0")]
        public int GoalDays { get; set; }

        [Display(Name = "Статус")]
        [Required]
        public required string Status { get; set; }

        public string? UserId { get; set; }

        public User? User { get; set; }

        public ICollection<HabitProgress> ProgressRecords { get; set; }
            = new List<HabitProgress>();

        public ICollection<HabitTag> HabitTags { get; set; }
            = new List<HabitTag>();

        public DateTime UpdatedAt { get; set; }
            = DateTime.UtcNow;
    }
}