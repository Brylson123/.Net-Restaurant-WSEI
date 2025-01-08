using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Restaurant.models;

namespace Restaurant.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Uzytkownik> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<Uzytkownik> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> RedirectBasedOnRole()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "AdminPanel");
                }

                return RedirectToAction("Create", "Zamowienia");
            }

            return RedirectToAction("Login", "Account");
        }
    }
}
