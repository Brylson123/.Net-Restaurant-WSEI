using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.models;
using System.Security.Claims;

namespace Restaurant
{
    [Authorize(Roles = "User, Admin")]
    public class ZamowieniaController : Controller
    {
        private readonly RestaurantDbContext _context;

        public ZamowieniaController(RestaurantDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create()
        {
            return View(await _context.Dania.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Dictionary<int, int> wybrane)
        {
            if (wybrane == null || !wybrane.Any(x => x.Value > 0))
                return View(await _context.Dania.ToListAsync());

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var z = new Zamowienie
            {
                UzytkownikId = userId,
                DataUtworzenia = DateTime.Now,
                Pozycje = new List<PozycjaZamowienia>()
            };

            foreach (var w in wybrane.Where(x => x.Value > 0))
            {
                z.Pozycje.Add(new PozycjaZamowienia { DanieId = w.Key, Ilosc = w.Value });
            }

            _context.Zamowienia.Add(z);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = z.Id });
        }

        public async Task<IActionResult> Details(int id)
        {
            var zamowienie = await _context.Zamowienia
                .Include(x => x.Pozycje)
                .ThenInclude(p => p.Danie)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (zamowienie == null) return NotFound();

            return View(zamowienie);
        }
        public async Task<IActionResult> History()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var zamowienia = await _context.Zamowienia
                .Where(z => z.UzytkownikId == userId)
                .Include(z => z.Pozycje)
                .ThenInclude(p => p.Danie)
                .ToListAsync();

            return View(zamowienia);
        }
    }
}
