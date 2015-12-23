using DatenMeister.Runtime.ExtentStorage;

namespace DatenMeister.CSV.Runtime.Storage
{
    public class CSVStorageConfiguration : ExtentFileStorageConfiguration
    {
        public CSVStorageConfiguration()
        {
            Settings = new CSVSettings();
        } 

        public CSVSettings Settings { get; private set; }
    }
}