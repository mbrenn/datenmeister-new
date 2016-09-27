using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Provider.CSV.Runtime.Storage
{
    public class CSVStorageConfiguration : ExtentFileStorageConfiguration
    {
        public CSVSettings Settings { get; set; } = new CSVSettings();

        public override string ToString()
        {
            if (Settings != null)
            {
                return $"CSVStorageConfiguration: {ExtentUri}, {Path}";
            }

            return base.ToString();
        }
    }
}