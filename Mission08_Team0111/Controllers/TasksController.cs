using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mission08_Team0111.Models;
using Mission08_Team0111.Data;
using Mission08_Team0111.ViewModels;

namespace Mission08_Team0111.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskRepository _taskRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ILogger<TasksController> _logger;

        public TasksController(
            ITaskRepository taskRepo,
            ICategoryRepository categoryRepo,
            ILogger<TasksController> logger)
        {
            _taskRepo = taskRepo ?? throw new ArgumentNullException(nameof(taskRepo));
            _categoryRepo = categoryRepo ?? throw new ArgumentNullException(nameof(categoryRepo));
            _logger = logger;
        }

        // ===============================
        // QUADRANTS PAGE (Main View)
        // ===============================
        public async Task<IActionResult> Index()
        {
            var tasks = (await _taskRepo.GetIncompleteAsync()).ToList();

            var vm = new QuadrantsViewModel
            {
                QuadrantI = tasks.Where(t => t.Quadrant == Quadrant.I).ToList(),
                QuadrantII = tasks.Where(t => t.Quadrant == Quadrant.II).ToList(),
                QuadrantIII = tasks.Where(t => t.Quadrant == Quadrant.III).ToList(),
                QuadrantIV = tasks.Where(t => t.Quadrant == Quadrant.IV).ToList()
            };

            return View(vm);
        }

        // ===============================
        // CREATE
        // ===============================
        public async Task<IActionResult> Create()
        {
            var vm = new TaskViewModel
            {
                Categories = (await _categoryRepo.GetAllAsync()).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = (await _categoryRepo.GetAllAsync()).ToList();
                return View(vm);
            }

            var task = new TaskItem
            {
                TaskText = vm.TaskText?.Trim(),
                DueDate = vm.DueDate,
                Quadrant = vm.Quadrant,
                CategoryId = vm.CategoryId,
                Completed = false
            };

            try
            {
                await _taskRepo.AddAsync(task);
                TempData["SuccessMessage"] = "Task created.";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error creating task");
                TempData["ErrorMessage"] = "An error occurred while creating the task.";
                vm.Categories = (await _categoryRepo.GetAllAsync()).ToList();
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // EDIT
        // ===============================
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskRepo.GetByIdAsync(id);
            if (task == null) return NotFound();

            var vm = new TaskViewModel
            {
                TaskItemId = task.TaskItemId,
                TaskText = task.TaskText,
                DueDate = task.DueDate,
                Quadrant = task.Quadrant,
                CategoryId = task.CategoryId,
                Completed = task.Completed,
                Categories = (await _categoryRepo.GetAllAsync()).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskViewModel vm)
        {
            if (id != vm.TaskItemId) return BadRequest();

            if (!ModelState.IsValid)
            {
                vm.Categories = (await _categoryRepo.GetAllAsync()).ToList();
                return View(vm);
            }

            var existingTask = await _taskRepo.GetByIdAsync(id);
            if (existingTask == null) return NotFound();

            existingTask.TaskText = vm.TaskText?.Trim();
            existingTask.DueDate = vm.DueDate;
            existingTask.Quadrant = vm.Quadrant;
            existingTask.CategoryId = vm.CategoryId;
            existingTask.Completed = vm.Completed;

            try
            {
                await _taskRepo.UpdateAsync(existingTask);
                TempData["SuccessMessage"] = "Task updated.";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error updating task id {TaskId}", id);
                TempData["ErrorMessage"] = "An error occurred while updating the task.";
                vm.Categories = (await _categoryRepo.GetAllAsync()).ToList();
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // DELETE
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _taskRepo.GetByIdAsync(id);
            if (existing == null)
            {
                TempData["ErrorMessage"] = "Task not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _taskRepo.DeleteAsync(id);
                TempData["SuccessMessage"] = "Task deleted.";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error deleting task id {TaskId}", id);
                TempData["ErrorMessage"] = "Could not delete the task.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // MARK COMPLETE
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkComplete(int id)
        {
            var existing = await _taskRepo.GetByIdAsync(id);
            if (existing == null)
            {
                TempData["ErrorMessage"] = "Task not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _taskRepo.MarkCompleteAsync(id);
                TempData["SuccessMessage"] = "Task marked complete.";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error marking task complete id {TaskId}", id);
                TempData["ErrorMessage"] = "Could not mark the task complete.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}