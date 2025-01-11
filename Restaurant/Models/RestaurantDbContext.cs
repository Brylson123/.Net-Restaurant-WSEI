using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Models
{
    public class RestaurantDbContext : IdentityDbContext<Uzytkownik>
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options) { }

        public DbSet<KategoriaDania>? Kategorie { get; set; }
        public DbSet<Danie>? Dania { get; set; }
        public DbSet<Zamowienie>? Zamowienia { get; set; }
        public DbSet<PozycjaZamowienia>? PozycjeZamowienia { get; set; }
    }
}
