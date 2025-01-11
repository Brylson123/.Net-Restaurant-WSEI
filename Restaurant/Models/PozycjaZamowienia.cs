namespace Restaurant.Models
{
    public class PozycjaZamowienia
    {
        public int Id { get; set; }
        public int ZamowienieId { get; set; }
        public Zamowienie? Zamowienie { get; set; }
        public int DanieId { get; set; }
        public Danie? Danie { get; set; }
        public int Ilosc { get; set; }
    }
}
