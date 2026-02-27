using Microsoft.EntityFrameworkCore;
using Mission08_Team0111.Models;

namespace Mission08_Team0111.Data
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskContext _context;

        public TaskRepository(TaskContext context)
        {
            _context = context;
        }

        public IEnumerable<Models.Task> GetAllTasks(bool includeCompleted = false)
        {
            var query = _context.Tasks.Include(t => t.Category).AsQueryable();
            if (!includeCompleted)
                query = query.Where(t => !t.Completed);
            return query.OrderBy(t => t.Quadrant).ThenBy(t => t.DueDate).ToList();
        }

        public IEnumerable<Models.Task> GetIncompleteTasks()
        {
            return _context.Tasks
                .Include(t => t.Category)
                .Where(t => !t.Completed)
                .OrderBy(t => t.Quadrant)
                .ThenBy(t => t.DueDate)
                .ToList();
        }

        public Models.Task? GetTaskById(int id)
        {
            return _context.Tasks.Include(t => t.Category).FirstOrDefault(t => t.TaskId == id);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.OrderBy(c => c.CategoryName).ToList();
        }

        public void AddTask(Models.Task task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public void UpdateTask(Models.Task task)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
        }

        public void DeleteTask(Models.Task task)
        {
            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
