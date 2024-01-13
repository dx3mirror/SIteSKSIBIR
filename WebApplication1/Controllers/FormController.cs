using Microsoft.AspNetCore.Mvc;
using System.Text;
using WebApplication1.Models;
using WebApplication1.UnitOfWork;
using WebApplication2;

namespace WebApplication1.Controllers
{
    public class FormController : Controller
    {
        private readonly IRequestUnitOfWork _unitOfWork;
        private readonly ILogger<FormController> _logger;
        private readonly KadrovikContext _context;

        public FormController(ILogger<FormController> logger,
            KadrovikContext context,
            IRequestUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            try
            {
                var requestSite = new RequestSite
                {
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    Email = model.Email,
                    About = model.About
                };

                _unitOfWork.RequestSiteRepository.Add(requestSite);

                _unitOfWork.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                StringBuilder errorMessage = new StringBuilder("Произошла ошибка при обработке вашего запроса. ");
                errorMessage.Append(ex.Message);

                ModelState.AddModelError(string.Empty, errorMessage.ToString());
                return View();
            }
        }

    }
}
