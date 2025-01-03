namespace Restaurant
{
    [Authorize(Roles="Admin")]
    public class DaniaController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DaniaController(ApplicationDbContext context) { _context = context; }
        public async Task<IActionResult> Index() { return View(await _context.Dania.Include(x=>x.KategoriaDania).ToListAsync()); }
        public async Task<IActionResult> Create()
        {
            ViewBag.Kategorie = await _context.Kategorie.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Danie d)
        {
            if(ModelState.IsValid)
            {
                _context.Dania.Add(d);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Kategorie = await _context.Kategorie.ToListAsync();
            return View(d);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var danie = await _context.Dania.FindAsync(id);
            if(danie==null) return NotFound();
            ViewBag.Kategorie = await _context.Kategorie.ToListAsync();
            return View(danie);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Danie d)
        {
            if(!ModelState.IsValid) return View(d);
            _context.Dania.Update(d);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var danie = await _context.Dania.FindAsync(id);
            if(danie==null) return NotFound();
            _context.Dania.Remove(danie);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
