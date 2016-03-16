namespace DatenMeister.Apps.ZipCode.Model
{
    public class ZipCode
    {
        public string Id { get; set; }
        public string Zip { get; set; }
        public string PositionLong { get; set; }
        public string PositionLat { get; set; }
        public string CityName { get; set; }
    }
}