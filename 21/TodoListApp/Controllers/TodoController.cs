using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Data;
using TodoListApp.Models;

namespace TodoListApp.Controllers
{
    public class TodoController : Controller
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ToDoItems.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(item);
        }

        public async Task<IActionResult> ToggleComplete(int id)
        {
            var task = await _context.ToDoItems.FindAsync(id);

            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.ToDoItems.FindAsync(id);

            if (task != null)
            {
                _context.ToDoItems.Remove(task);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}