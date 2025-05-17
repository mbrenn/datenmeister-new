namespace ZipCodeLibrary
{
    public class ZipCodeModel
    {
        public List<ZipCodeData> items { get; set; } = new();
        
        public bool truncated { get; set; }
        
        public bool noItemFound { get; set; }
    }

    public class ZipCodeData
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public string zip { get; set; } = "";
        public double positionLong { get; set; }
        public double positionLat { get; set; }
    }
}