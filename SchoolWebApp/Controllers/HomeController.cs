using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolWebApp.Models;
using System.Diagnostics;

namespace SchoolWebApp.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager) {
            _logger = logger;
            _userManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> Index() {
            AppUser? signedInUser = await _userManager.GetUserAsync(HttpContext.User);
            return View("Index", signedInUser.UserName);
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
