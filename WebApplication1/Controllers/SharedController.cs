using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Models;
using WebApplication2;

namespace WebApplication1.Controllers
{
    public class SharedController : Controller
    {
     
        public SharedController()
        {
           
        }
        public IActionResult Index()
        {
            return View();
        }
       

        
    }
}
