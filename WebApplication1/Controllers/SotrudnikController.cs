using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class SotrudnikController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            Console.WriteLine("Index method called");
            return View();
        }
        
        
    }
}
