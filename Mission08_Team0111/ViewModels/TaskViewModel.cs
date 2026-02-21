using System.ComponentModel.DataAnnotations;
using Mission08_Team0111.Models;

namespace Mission08_Team0111.ViewModels
{
    public class TaskViewModel
    {
        public int TaskItemId { get; set; }

        [Required]
        public string TaskText { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public Quadrant Quadrant { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public bool Completed { get; set; }

        // This is for dropdown population
        public List<Category> Categories { get; set; } = new();
    }
}