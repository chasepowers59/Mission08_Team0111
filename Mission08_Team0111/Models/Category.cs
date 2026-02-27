namespace Mission08_Team0111.Models
{
    public class Category
    {
namespace Mission08_Team0111.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
