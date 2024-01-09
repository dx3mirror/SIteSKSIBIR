using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Models;
using Microsoft.Extensions.Logging;
using WebApplication2;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace WebApplication1.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly KadrovikContext _dbContext;
        private readonly ILogger<AuthorizationController> _logger;
        public AuthorizationController(ILogger<AuthorizationController> logger, KadrovikContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _dbContext.UsersSites.FirstOrDefault(u => u.Login == username);

            if (user != null && user.Password == password)
            {
                _logger.LogInformation($"Пользователь {username} успешно вошел в систему.");

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            // ... добавьте другие требуемые утверждения
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30), // Время истечения срока действия аутентификации
                    IsPersistent = false, // Указывает, будет ли аутентификация сохраняться после закрытия браузера
                    RedirectUri = "/Home/Index",
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return Json(new { success = true });
            }

            // Если авторизация не удалась, возвращаем сообщение об ошибке
            return Json(new { success = false, errorMessage = "Неверный логин или пароль" });
        }


    }
}
