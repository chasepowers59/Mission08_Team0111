using Mission08_Team0111.Models;

namespace Mission08_Team0111.ViewModels
{
    public class QuadrantsViewModel
    {
        public List<TaskItem> QuadrantI { get; set; } = new();
        public List<TaskItem> QuadrantII { get; set; } = new();
        public List<TaskItem> QuadrantIII { get; set; } = new();
        public List<TaskItem> QuadrantIV { get; set; } = new();
    }
}