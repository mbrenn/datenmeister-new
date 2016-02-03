using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.CSV.Runtime.Storage
{
    public class CSVStorageConfiguration : ExtentFileStorageConfiguration
    {
        public CSVStorageConfiguration()
        {
        } 

        public CSVSettings Settings { get; set; } = new CSVSettings();
    }
}