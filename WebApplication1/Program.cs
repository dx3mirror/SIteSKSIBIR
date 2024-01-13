using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Factory;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.Strategy;
using WebApplication1.UnitOfWork;
using WebApplication2;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контекст данных для работы с базой данных
builder.Services.AddDbContext<KadrovikContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMemoryCache();
builder.Services.AddSession(); // Добавляем сервис сессии

builder.Services.AddScoped<IFailedLoginAttemptRepository, FailedLoginAttemptRepository>();
builder.Services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
builder.Services.AddScoped<ISignInStrategy, DefaultSignInStrategy>();
builder.Services.AddScoped<IRequestUnitOfWork, RequestUnitOfWork>();
builder.Services.AddScoped<IFindSotrudnikUnitOfWork, FindSotrudnikUnitOfWork>();
builder.Services.AddScoped<IFileConversionStrategy, MemoryConversionStrategy>();
builder.Services.AddScoped<IAddSotrudnikUnitOfWork, AddSotrudnikUnitOfWork>();
builder.Services.AddScoped<IAddSotrudnikRepository, SotrudnikRepository>();
builder.Services.AddScoped<ISotrudnikUpdateUnitOfWork, SotrudnikUpdateUnitOfWork>();
builder.Services.AddScoped<ISotrudnikUpdateRepository, SotrudnikUpdateRepository>();
builder.Services.AddScoped<IDoljnostUnitOfWork, DoljnostUnitOfWork>();
builder.Services.AddScoped<IDoljnostRepository, DoljnostRepository>();
builder.Services.AddScoped<IUpdateDoljnostUnitOfWork, UpdateDoljnostUnitOfWork>();
builder.Services.AddScoped<IUpdateDoljnostRepository, UpdateDoljnostRepository>();
builder.Services.AddScoped<IAddDoljstostRepository, AddDoljnostRepository>();
builder.Services.AddScoped<IAddDoljnostUnitOfWork, AddDoljnostUnitOfWork>();
builder.Services.AddScoped<IDoljnostFactory, DoljnostFactory>();
builder.Services.AddScoped(typeof(IOtpuskViewRepository<>), typeof(ViewOtpuskRepository<>));
builder.Services.AddScoped<IOtpuskViewRepository<Otpusk> ,ViewOtpuskRepository<Otpusk>>();
builder.Services.AddScoped<IViewOtpuskUnitOfWork, ViewOtpuskUnitofWork>();
builder.Services.AddScoped<IOtpuskViewModelFactory, OtpuskViewModelFactory>();

// Настройка авторизации
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "YourAppAuthCookie";
        options.LoginPath = "/Authorization/Index"; // Путь к странице логина
    });

builder.Services.AddHttpContextAccessor();
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

// Настройка сессий
app.UseSession();

// Настройка авторизации и аутентификации
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

// Запуск приложения
app.Run();
