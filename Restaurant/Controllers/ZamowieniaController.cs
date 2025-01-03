 namespace Restaurant
{
    [Authorize]
    public class ZamowieniaController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ZamowieniaController(ApplicationDbContext context) { _context = context; }
        public async Task<IActionResult> Create()
        {
            return View(await _context.Dania.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Dictionary<int,int> wybrane)
        {
            if(wybrane==null || !wybrane.Any(x=>x.Value>0)) return View(await _context.Dania.ToListAsync());
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var z = new Zamowienie { UzytkownikId = userId, DataUtworzenia=DateTime.Now, Pozycje=new List<PozycjaZamowienia>() };
            foreach(var w in wybrane.Where(x=>x.Value>0))
            {
                z.Pozycje.Add(new PozycjaZamowienia{DanieId=w.Key,Ilosc=w.Value});
            }
            _context.Zamowienia.Add(z);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details",new{id=z.Id});
        }
        public async Task<IActionResult> Details(int id)
        {
            var zamowienie = await _context.Zamowienia.Include(x=>x.Pozycje).ThenInclude(p=>p.Danie).FirstOrDefaultAsync(x=>x.Id==id);
            if(zamowienie==null) return NotFound();
            return View(zamowienie);
        }
    }
}
