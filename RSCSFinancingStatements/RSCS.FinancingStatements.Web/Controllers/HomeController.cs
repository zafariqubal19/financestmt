using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RSCS.FinancingStatements.Security.Base;
using RSCS.FinancingStatements.Web.Facade;
using RSCS.FinancingStatements.Web.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace RSCS.FinancingStatements.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApplicationFacade _applicationFacade;

        public HomeController(ILogger<HomeController> logger, IApplicationFacade applicationFacade)
        {
            _logger = logger;
            _applicationFacade = applicationFacade;
        }

        public IActionResult Index()
        {
            _logger.LogInformation($"Index Page Initiated");
            _logger.LogError(new Exception("Custom Exception"), $"Additional Message...");

            var someSettings = _applicationFacade.FetchSomeSettings();
            var indexViewModel = new IndexViewModel
            {
                User = new User
                {
                    ID = AppUser.Username,
                    FirstName = AppUser.FirstName,
                    MiddleName = AppUser.MiddleName,
                    LastName = AppUser.LastName,
                    Email = AppUser.Email

                },
                Settings = (IEnumerable<Models.Setting>)someSettings
            };
            return View(indexViewModel);


        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
	
		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Account");
		}

		[HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Check if the provided username and password match the Windows authenticated user
                if (username == User.Identity.Name)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, username) };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("GetFinPrograms", "Invoice");
                }
            }
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

   

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}