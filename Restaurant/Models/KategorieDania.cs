using System;

namespace Restaurant
{
    public class KategoriaDania
    {
        public int Id { get; set; }
        public string? Nazwa { get; set; }
        public List<Danie>? Dania { get; set; }
    }
}
