using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurant.models;

namespace Restaurant
{
    public class AccountController : Controller
    {
        private readonly UserManager<Uzytkownik> _um;
        private readonly SignInManager<Uzytkownik> _sm;

        public AccountController(UserManager<Uzytkownik> um, SignInManager<Uzytkownik> sm)
        {
            _um = um;
            _sm = sm;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Email i has³o s¹ wymagane.");
                return View();
            }

            var user = new Uzytkownik { UserName = email, Email = email };
            var result = await _um.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _um.AddToRoleAsync(user, "User");
                await _sm.SignInAsync(user, false);
                return RedirectToAction("Create", "Zamowienia");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Email i has³o s¹ wymagane.");
                return View();
            }

            var result = await _sm.PasswordSignInAsync(email, password, false, false);

            if (result.Succeeded)
            {
                var user = await _um.FindByEmailAsync(email);
                var roles = await _um.GetRolesAsync(user);

                if (roles.Contains("Admin"))
                {
                    return RedirectToAction("Index", "AdminPanel");
                }

                if (roles.Contains("User"))
                {
                    return RedirectToAction("Create", "Zamowienia");
                }
            }

            ModelState.AddModelError(string.Empty, "Nieprawid³owy email lub has³o.");
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await _sm.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
