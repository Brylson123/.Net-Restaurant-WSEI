using System;

namespace Restaurant.models
{
    public class KategoriaDania
    {
        public int Id { get; set; }
        public string? Nazwa { get; set; }
        public List<Danie>? Dania { get; set; }
    }
}
