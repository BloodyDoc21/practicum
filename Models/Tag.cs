using System.ComponentModel.DataAnnotations;

namespace CleanLife.Web.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        public string OwnerId { get; set; }
        public User? Owner { get; set; }

        public ICollection<HabitTag> HabitTags { get; set; } = new List<HabitTag>();

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
