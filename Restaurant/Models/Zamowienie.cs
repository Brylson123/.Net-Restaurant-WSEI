namespace Restaurant.Models
{
    public class Zamowienie
    {
        public int Id { get; set; }
        public string? UzytkownikId { get; set; }
        public Uzytkownik? Uzytkownik { get; set; }
        public DateTime DataUtworzenia { get; set; } = DateTime.Now;
        public string? Adres { get; set; }

        public List<PozycjaZamowienia>? Pozycje { get; set; }
    }
}
