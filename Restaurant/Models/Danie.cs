namespace Restaurant.models
{
    public class Danie
    {
        public int Id { get; set; }
        public string? Nazwa { get; set; }
        public decimal Cena { get; set; }
        public int KategoriaDaniaId { get; set; }
        public KategoriaDania? KategoriaDania { get; set; }
    }
}
