using System.ComponentModel.DataAnnotations;

namespace CleanLife.Web.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string? Avatar { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? Settings { get; set; }

        public ICollection<Habit> Habits { get; set; } = new List<Habit>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
