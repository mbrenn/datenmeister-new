using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

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