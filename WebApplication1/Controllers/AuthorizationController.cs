using Microsoft.AspNetCore.Mvc;
using WebApplication2;
using WebApplication1.Repository;
using WebApplication1.UnitOfWork;
using WebApplication1.Strategy;

namespace WebApplication1.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly KadrovikContext _dbContext;
        private readonly IAppUnitOfWork _unitOfWork;
        private readonly ILogger<AuthorizationController> _logger;
        private readonly IFailedLoginAttemptRepository _failedLoginAttemptRepository;
        private readonly ISignInStrategy _signInStrategy;
        public AuthorizationController(IFailedLoginAttemptRepository failedLoginAttemptRepository,
            IAppUnitOfWork unitOfWork,
            ILogger<AuthorizationController> logger,
            KadrovikContext dbContext,
            ISignInStrategy signInStrategy)
        {
            _unitOfWork = unitOfWork;
            _failedLoginAttemptRepository = failedLoginAttemptRepository;
            _dbContext = dbContext;
            _logger = logger;
            _signInStrategy = signInStrategy;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _unitOfWork.UserRepository.FindUserAsync(username, password);

            if (user != null)
            {
                _logger.LogInformation($"User {username} successfully logged in.");

                await _signInStrategy.SignInAsync(HttpContext, username);

                _failedLoginAttemptRepository.ResetFailedLoginAttempts(username);

                return Json(new { success = true });
            }
            else
            {
                _failedLoginAttemptRepository.IncrementFailedLoginAttempts(username);

                if (_failedLoginAttemptRepository.IsAccountLocked(username))
                {
                    return Json(new { success = false, errorMessage = "Account is locked due to multiple unsuccessful login attempts. Please try again later." });
                }

                return Json(new { success = false, errorMessage = "Incorrect username or password" });
            }
        }


    }
}
