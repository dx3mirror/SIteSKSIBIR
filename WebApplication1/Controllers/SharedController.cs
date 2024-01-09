using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Models;
using WebApplication2;

namespace WebApplication1.Controllers
{
    public class SharedController : Controller
    {
        private readonly KadrovikContext _dbContext;
        public SharedController(KadrovikContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            // Хешируем введенный пароль
            byte[] hashedInputPassword;
            using (SHA256 sha256 = SHA256.Create())
            {
                hashedInputPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            // Получаем пользователя из базы данных по логину
            var user = _dbContext.Users.FirstOrDefault(u => u.Login.SequenceEqual(Encoding.UTF8.GetBytes(username)));

            // Если пользователь найден и хешированный пароль совпадает
            if (user != null && user.Password.SequenceEqual(hashedInputPassword))
            {
                // Авторизация успешна
                // Вы можете добавить дополнительную логику здесь, например, установку куки или идентификатора сеанса

                // Перенаправляем на страницу Index в папке Views/Sotrudnik
                return Json(new { success = true });
            }

            // Если авторизация не удалась, возвращаем сообщение об ошибке
            return Json(new { success = false, errorMessage = "Неверный логин или пароль" });
        }

        [HttpPost]
        public async Task<IActionResult> AddSotrudnik(Sotrudnik model, IFormFile avatarFile)
        {
            Console.WriteLine("HttpPost method called");

            // Set other properties from the form
            model.Lastname = Request.Form["Lastname"];
            model.Firstname = Request.Form["Firstname"];
            model.Patranomic = Request.Form["Patranomic"];
            model.Adress = Request.Form["Adress"];
            model.MestoRojd = Request.Form["MestoRojd"];

            // Validate and set Datarojdeniy
            if (DateTime.TryParse(Request.Form["Datarojdeniy"], out DateTime parsedDate))
            {
                model.Datarojdeniy = parsedDate;
            }
            else
            {
                model.Datarojdeniy = null;
            }

            model.IdentityNomer = Request.Form["IdentityNomer"];
            model.Okin = Request.Form["Okin"];

            if (avatarFile != null && avatarFile.Length > 0)
            {
                model.Avatar = await ConvertFileToByteArrayAsync(avatarFile);
            }

            _dbContext.Sotrudniks.Add(model);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        private async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        private bool VerifyPassword(string inputPassword, byte[] storedHash)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));

                // Сравниваем два массива байтов
                return storedHash.SequenceEqual(hashedBytes);
            }
        }
    }
}
