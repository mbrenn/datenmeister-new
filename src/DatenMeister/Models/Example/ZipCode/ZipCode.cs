// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace DatenMeister.Models.Example.ZipCode
{
    public class ZipCode
    {
        public int id { get; set; }
        public int zip { get; set; }
        public double positionLong { get; set; }
        public double positionLat { get; set; }
        public string name { get; set; }
    }
}