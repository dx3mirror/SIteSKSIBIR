using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication2;

var builder = WebApplication.CreateBuilder(args);

try
{
    // Добавляем контекст данных для работы с базой данных
    builder.Services.AddDbContext<KadrovikContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
catch (Exception ex)
{
    Console.WriteLine($"Error loading database context: {ex.Message}");
    throw; // Пробрасываем исключение дальше
}
// Настройка авторизации
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "YourAppAuthCookie";
        options.LoginPath = "/Authorization/Index"; // Путь к странице логина
    });

builder.Services.AddHttpContextAccessor();
// Добавляем контроллеры и представления
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Настройка конвейера обработки HTTP-запросов.
if (!app.Environment.IsDevelopment())
{
    // В случае ошибки, перенаправляем на страницу ошибок
    app.UseExceptionHandler("/Home/Error");

    // Задаем параметры HSTS (HTTP Strict Transport Security)
    app.UseHsts();
}

// Перенаправление с HTTP на HTTPS
app.UseHttpsRedirection();

// Разрешаем использование статических файлов из wwwroot
app.UseStaticFiles();

// Настройка маршрутизации
app.UseRouting();


// Настройка авторизации
builder.Services.AddAuthorization();
// Настройка конвейера обработки HTTP-запросов.
if (!app.Environment.IsDevelopment())
{
    // В случае ошибки, перенаправляем на страницу ошибок
    app.UseExceptionHandler("/Home/Error");

    // Задаем параметры HSTS (HTTP Strict Transport Security)
    app.UseHsts();
}
builder.Services.AddHttpContextAccessor();
app.UseAuthentication();
app.UseAuthorization();

// Настройка маршрутов контроллеров
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Добавляем обработку запросов к index.html вместо Index.cshtml
app.MapFallbackToFile("index.html");

// Для запроса к корневому пути, отправляем index.html из wwwroot
app.MapGet("/", async context =>
{
    await context.Response.SendFileAsync("wwwroot/index.html");
});
app.UseStaticFiles();

// Запуск приложения
app.Run();
