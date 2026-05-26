using Microsoft.AspNetCore.Identity;
using CleanLife.Web.Models;

namespace CleanLife.Web.Models
{
    public class User : IdentityUser
    {
        public string? Avatar { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Settings { get; set; }

        // Навигационные свойства
        public ICollection<Habit> Habits { get; set; } = new List<Habit>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
