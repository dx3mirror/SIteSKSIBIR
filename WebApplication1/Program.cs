using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication2;

var builder = WebApplication.CreateBuilder(args);

try
{
    // ��������� �������� ������ ��� ������ � ����� ������
    builder.Services.AddDbContext<KadrovikContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
catch (Exception ex)
{
    Console.WriteLine($"Error loading database context: {ex.Message}");
    throw; // ������������ ���������� ������
}
// ��������� �����������
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "YourAppAuthCookie";
        options.LoginPath = "/Authorization/Index"; // ���� � �������� ������
    });

builder.Services.AddHttpContextAccessor();
// ��������� ����������� � �������������
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ��������� ��������� ��������� HTTP-��������.
if (!app.Environment.IsDevelopment())
{
    // � ������ ������, �������������� �� �������� ������
    app.UseExceptionHandler("/Home/Error");

    // ������ ��������� HSTS (HTTP Strict Transport Security)
    app.UseHsts();
}

// ��������������� � HTTP �� HTTPS
app.UseHttpsRedirection();

// ��������� ������������� ����������� ������ �� wwwroot
app.UseStaticFiles();

// ��������� �������������
app.UseRouting();


// ��������� �����������
builder.Services.AddAuthorization();
// ��������� ��������� ��������� HTTP-��������.
if (!app.Environment.IsDevelopment())
{
    // � ������ ������, �������������� �� �������� ������
    app.UseExceptionHandler("/Home/Error");

    // ������ ��������� HSTS (HTTP Strict Transport Security)
    app.UseHsts();
}
builder.Services.AddHttpContextAccessor();
app.UseAuthentication();
app.UseAuthorization();

// ��������� ��������� ������������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ��������� ��������� �������� � index.html ������ Index.cshtml
app.MapFallbackToFile("index.html");

// ��� ������� � ��������� ����, ���������� index.html �� wwwroot
app.MapGet("/", async context =>
{
    await context.Response.SendFileAsync("wwwroot/index.html");
});
app.UseStaticFiles();

// ������ ����������
app.Run();
