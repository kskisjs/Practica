using Microsoft.AspNetCore.Mvc;
using TodoListApp.Services;
using TodoListApp.ViewModels;

namespace TodoListApp.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITaskService _taskService;

        // DI: сервис внедряется через конструктор
        public TodoController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: /Todo?filter=all|active|completed
        public IActionResult Index(string filter = "all")
        {
            var tasks = _taskService.GetTasks();

            var filtered = filter switch
            {
                "active"    => tasks.Where(t => !t.IsCompleted).ToList(),
                "completed" => tasks.Where(t =>  t.IsCompleted).ToList(),
                _           => tasks
            };

            ViewBag.Filter      = filter;
            ViewBag.TotalCount  = tasks.Count;
            ViewBag.DoneCount   = tasks.Count(t => t.IsCompleted);
            ViewBag.ActiveCount = tasks.Count(t => !t.IsCompleted);

            // Уведомление о завершении задачи через ViewBag (читаем TempData)
            ViewBag.Notification = TempData["Notification"];

            return View(filtered);
        }

        // GET: /Todo/Add
        public IActionResult Add()
        {
            return View(new TaskViewModel());
        }

        // POST: /Todo/Add  — форма с валидацией
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TaskViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new TaskDto
            {
                Description = model.Description,
                DueDate     = model.DueDate,
                IsCompleted = false
            };

            _taskService.AddTask(dto);
            return RedirectToAction("Index");
        }

        // GET: /Todo/Complete/1 — пометить задачу выполненной
        public IActionResult Complete(int id)
        {
            var task = _taskService.GetById(id);
            if (task != null)
            {
                _taskService.CompleteTask(id);
                // Уведомление передаём через TempData → в Index читаем в ViewBag
                TempData["Notification"] = $"Задача «{task.Description}» отмечена как выполненная!";
            }

            return RedirectToAction("Index");
        }
    }
}
