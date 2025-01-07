using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurant.models;

public class DaniaController : Controller
{
    private readonly RestaurantDbContext _context;

    public DaniaController(RestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var dania = await _context.Dania.Include(d => d.KategoriaDania).ToListAsync();
        return View(dania);
    }

    public IActionResult Create()
    {
        ViewBag.Kategorie = new SelectList(_context.Kategorie, "Id", "Nazwa");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Danie danie)
    {
        if (ModelState.IsValid)
        {
            _context.Dania.Add(danie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Kategorie = new SelectList(_context.Kategorie, "Id", "Nazwa");
        return View(danie);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var danie = await _context.Dania.FindAsync(id);
        if (danie == null) return NotFound();

        ViewBag.Kategorie = new SelectList(_context.Kategorie, "Id", "Nazwa", danie.KategoriaDaniaId);
        return View(danie);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Danie danie)
    {
        if (id != danie.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(danie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Kategorie = new SelectList(_context.Kategorie, "Id", "Nazwa", danie.KategoriaDaniaId);
        return View(danie);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var danie = await _context.Dania.Include(d => d.KategoriaDania).FirstOrDefaultAsync(m => m.Id == id);
        if (danie == null) return NotFound();

        return View(danie);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var danie = await _context.Dania.FindAsync(id);
        if (danie != null)
        {
            _context.Dania.Remove(danie);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
