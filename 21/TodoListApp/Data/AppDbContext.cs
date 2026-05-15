using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TodoListApp.Models;

namespace TodoListApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TodoItem> ToDoItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}