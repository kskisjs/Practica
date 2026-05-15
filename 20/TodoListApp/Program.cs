using TodoListApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Регистрируем MVC
builder.Services.AddControllersWithViews();

// DI: регистрация ITaskService
builder.Services.AddSingleton<ITaskService, TaskService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "complete",
    pattern: "Todo/Complete/{id:int}",
    defaults: new { controller = "Todo", action = "Complete" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Todo}/{action=Index}/{id?}");

app.Run();
