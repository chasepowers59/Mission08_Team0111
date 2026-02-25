using Mission08_Team0111.Models;

namespace Mission08_Team0111.Data
{
    public interface ITaskRepository
    {
        IEnumerable<Models.Task> GetAllTasks(bool includeCompleted = false);
        IEnumerable<Models.Task> GetIncompleteTasks();
        Models.Task? GetTaskById(int id);
        IEnumerable<Category> GetCategories();
        void AddTask(Models.Task task);
        void UpdateTask(Models.Task task);
        void DeleteTask(Models.Task task);
        void SaveChanges();
    }
}
