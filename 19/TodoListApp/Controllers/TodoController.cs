using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models;

namespace TodoListApp.Controllers
{
    public class TodoController : Controller
    {
        // Имитация "базы данных" в памяти (static, чтобы данные сохранялись между запросами)
        private static List<TodoItem> _todos = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Купить продукты", IsCompleted = false },
            new TodoItem { Id = 2, Title = "Прочитать книгу", IsCompleted = true },
            new TodoItem { Id = 3, Title = "Сделать зарядку", IsCompleted = false },
        };
        private static int _nextId = 4;

        // GET: /Todo/Index  или  /Todo?filter=all|active|completed
        public IActionResult Index(string filter = "all")
        {
            var items = filter switch
            {
                "active"    => _todos.Where(t => !t.IsCompleted).ToList(),
                "completed" => _todos.Where(t =>  t.IsCompleted).ToList(),
                _           => _todos.ToList()
            };

            ViewBag.Filter      = filter;
            ViewBag.TotalCount  = _todos.Count;
            ViewBag.DoneCount   = _todos.Count(t => t.IsCompleted);
            ViewBag.ActiveCount = _todos.Count(t => !t.IsCompleted);

            return View(items);
        }

        // GET: /Todo/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: /Todo/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TodoItem item)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(item.Title))
            {
                ModelState.AddModelError("Title", "Введите название задачи.");
                return View(item);
            }

            item.Id = _nextId++;
            item.IsCompleted = false;
            _todos.Add(item);

            return RedirectToAction("Index");
        }

        // GET: /Todo/Complete/1  — переключить статус задачи
        public IActionResult Complete(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo != null)
                todo.IsCompleted = !todo.IsCompleted;

            return RedirectToAction("Index");
        }

        // GET: /Todo/Delete/1
        public IActionResult Delete(int id)
        {
            var todo = _todos.FirstOrDefault(t => t.Id == id);
            if (todo != null)
                _todos.Remove(todo);

            return RedirectToAction("Index");
        }
    }
}
