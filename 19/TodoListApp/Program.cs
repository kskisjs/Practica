var builder = WebApplication.CreateBuilder(args);

// Регистрируем MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

// Маршруты
app.MapControllerRoute(
    name: "complete",
    pattern: "Todo/Complete/{id:int}",
    defaults: new { controller = "Todo", action = "Complete" });

app.MapControllerRoute(
    name: "delete",
    pattern: "Todo/Delete/{id:int}",
    defaults: new { controller = "Todo", action = "Delete" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Todo}/{action=Index}/{id?}");

app.Run();
