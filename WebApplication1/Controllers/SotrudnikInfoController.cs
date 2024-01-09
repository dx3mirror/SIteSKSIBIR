using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication2;

namespace WebApplication1.Controllers
{
    public class SotrudnikInfoController : Controller
    {
        private readonly KadrovikContext _dbContext;
     
        public SotrudnikInfoController(KadrovikContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IActionResult AddSotrudnik() => View();

        

        public async Task<IActionResult> Index()
        {
         

            var sotrudniki = await _dbContext.Sotrudniks
                .OrderBy(s => s.Id) 
                .ToListAsync();
            Console.WriteLine("HttpPost method calledаывфафывафыа");

            return View(sotrudniki);
        }
        [HttpPost("/api/UpdateSotrudnik")]
        public async Task<IActionResult> UpdateSotrudnikWithPhoto([FromForm] Sotrudnik model, IFormFile photoChange)
        {
            try
            {
                Console.WriteLine($"ID: {model.Id}, LastnameChange: {model.Lastname}, ...");
                // Проверка наличия ID
                if (model.Id <= 0)
                {
                    return BadRequest("Invalid ID");
                }

                // Поиск сотрудника по ID
                var existingSotrudnik = await _dbContext.Sotrudniks.FindAsync(model.Id);

                if (existingSotrudnik == null)
                {
                    return BadRequest("Sotrudnik not found");
                }

                // Обновление данных сотрудника
                _dbContext.Entry(existingSotrudnik).CurrentValues.SetValues(model);

                // Обработка фотографии
                if (photoChange != null && photoChange.Length > 0)
                {
                    existingSotrudnik.Avatar = await ConvertFileToByteArrayAsync(photoChange);
                }

                await _dbContext.SaveChangesAsync();

                return Ok(new { success = true, id = existingSotrudnik.Id, /* другие обновленные данные */ });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpPost("/api/AddSotrudnik")]
        public async Task<IActionResult> AddSotrudnikWithPhoto([FromForm] Sotrudnik model, IFormFile photo)
        {
            try
            {
                // Обработка данных сотрудника
                if (ModelState.IsValid)
                {
                    _dbContext.Sotrudniks.Add(model);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return BadRequest("Invalid model state");
                }

                // Обработка фотографии
                if (photo != null && photo.Length > 0)
                {
                    model.Avatar = await ConvertFileToByteArrayAsync(photo);
                    await _dbContext.SaveChangesAsync();
                }

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        // Метод для конвертации файла в массив байтов
        private async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }


    }
    
    
}
