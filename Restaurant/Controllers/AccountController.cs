 namespace Restaurant
{
    public class AccountController : Controller
    {
        private readonly UserManager<Uzytkownik> _um;
        private readonly SignInManager<Uzytkownik> _sm;
        public AccountController(UserManager<Uzytkownik> um, SignInManager<Uzytkownik> sm){_um=um;_sm=sm;}
        public IActionResult Register(){return View();}
        [HttpPost]
        public async Task<IActionResult> Register(string email,string password)
        {
            if(string.IsNullOrEmpty(email)||string.IsNullOrEmpty(password)) return View();
            var user=new Uzytkownik{UserName=email,Email=email};
            var result=await _um.CreateAsync(user,password);
            if(result.Succeeded)
            {
                await _um.AddToRoleAsync(user,"User");
                await _sm.SignInAsync(user,false);
                return RedirectToAction("Index","Home");
            }
            return View();
        }
        public IActionResult Login(){return View();}
        [HttpPost]
        public async Task<IActionResult> Login(string email,string password)
        {
            var result=await _sm.PasswordSignInAsync(email,password,false,false);
            if(result.Succeeded)return RedirectToAction("Index","Home");
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _sm.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
