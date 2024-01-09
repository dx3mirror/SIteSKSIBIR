using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication2;

namespace WebApplication1.Controllers
{
    public class FormController : Controller
    {
        
        private readonly ILogger<FormController> _logger;
        private readonly KadrovikContext _context;

        public FormController(ILogger<FormController> logger, KadrovikContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
        }

        [HttpPost]
        public IActionResult ProcessForm(YourModelNamespace model)
        {

            _logger.LogInformation($"Received form data: Last Name - {model.LastName}, First Name - {model.FirstName}, Email - {model.Email}, About - {model.About}");

            var requestSite = new RequestSite
            {
                LastName = model.LastName,
                FirstName = model.FirstName,
                Email = model.Email,
                About = model.About
            };

            _context.RequestSites.Add(requestSite);

            // Сохраняем изменения в базе данных
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
