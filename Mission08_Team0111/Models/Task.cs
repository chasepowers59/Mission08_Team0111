using System.ComponentModel.DataAnnotations;

namespace Mission08_Team0111.Models
{
    public class Task
    {
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Task title is required.")]
        public string Title { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        [Required]
        [Range(1, 4, ErrorMessage = "Quadrant must be 1–4.")]
        public int Quadrant { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public bool Completed { get; set; }
    }
}
