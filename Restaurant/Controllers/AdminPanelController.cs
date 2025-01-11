using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : Controller
    {
        private readonly RestaurantDbContext _context;

        public AdminPanelController(RestaurantDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ManageDania()
        {
            var dania = await _context.Dania.Include(d => d.KategoriaDania).ToListAsync();
            return View(dania);
        }

        public async Task<IActionResult> ManageZamowienia()
        {
            var zamowienia = await _context.Zamowienia
                .Include(z => z.Uzytkownik)
                .Include(z => z.Pozycje)
                .ThenInclude(p => p.Danie)
                .ToListAsync();
            return View(zamowienia);
        }

        public async Task<IActionResult> ZamowienieDetails(int id)
        {
            var zamowienie = await _context.Zamowienia
                .Include(z => z.Uzytkownik)
                .Include(z => z.Pozycje)
                .ThenInclude(p => p.Danie)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zamowienie == null)
            {
                return NotFound();
            }

            return View(zamowienie);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteZamowienie(int id)
        {
            var zamowienie = await _context.Zamowienia.FindAsync(id);
            if (zamowienie != null)
            {
                _context.Zamowienia.Remove(zamowienie);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ManageZamowienia));
        }
    }
}
