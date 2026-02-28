using Microsoft.AspNetCore.Mvc;
using Mission08_Team0111.Data;
using TaskModel = Mission08_Team0111.Models.Task;

namespace Mission08_Team0111.Controllers
{
    // Handles task CRUD plus task completion actions for the app.
    public class TasksController : Controller
    {
        private readonly ITaskRepository _taskRepo;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskRepository taskRepo, ILogger<TasksController> logger)
        {
            _taskRepo = taskRepo ?? throw new ArgumentNullException(nameof(taskRepo));
            _logger = logger;
        }

        // Shows the Eisenhower matrix list view with only incomplete tasks.
        public IActionResult Index()
        {
            var tasks = _taskRepo.GetIncompleteTasks();
            return View(tasks);
        }

        // Displays the add form (reusing the existing Home/AddTask view).
        [HttpGet]
        public IActionResult Add()
        {
            return View("~/Views/Home/AddTask.cshtml", new TaskModel());
        }

        // Creates a new task from form input.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TaskModel task)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Home/AddTask.cshtml", task);
            }

            try
            {
                task.Title = task.Title.Trim();
                _taskRepo.AddTask(task);
                TempData["SuccessMessage"] = "Task created.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the task.");
                return View("~/Views/Home/AddTask.cshtml", task);
            }
        }

        // Loads the selected task for editing.
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _taskRepo.GetTaskById(id);
            if (task == null) return NotFound();

            return View(task);
        }

        // Persists changes to an existing task.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaskModel task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }

            var existingTask = _taskRepo.GetTaskById(task.TaskId);
            if (existingTask == null) return NotFound();

            existingTask.Title = task.Title.Trim();
            existingTask.DueDate = task.DueDate;
            existingTask.Quadrant = task.Quadrant;
            existingTask.CategoryId = task.CategoryId;
            existingTask.Completed = task.Completed;

            try
            {
                _taskRepo.UpdateTask(existingTask);
                TempData["SuccessMessage"] = "Task updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task id {TaskId}.", task.TaskId);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the task.");
                return View(task);
            }
        }

        // Deletes a task by id.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var existingTask = _taskRepo.GetTaskById(id);
            if (existingTask == null)
            {
                TempData["ErrorMessage"] = "Task not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _taskRepo.DeleteTask(existingTask);
                TempData["SuccessMessage"] = "Task deleted.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task id {TaskId}.", id);
                TempData["ErrorMessage"] = "Could not delete the task.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Marks a task complete so it no longer appears in the incomplete list.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkComplete(int id)
        {
            var existingTask = _taskRepo.GetTaskById(id);
            if (existingTask == null)
            {
                TempData["ErrorMessage"] = "Task not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                existingTask.Completed = true;
                _taskRepo.UpdateTask(existingTask);
                TempData["SuccessMessage"] = "Task marked complete.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking task complete id {TaskId}.", id);
                TempData["ErrorMessage"] = "Could not mark the task complete.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
