using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Provider.CSV.Runtime
{
    // ReSharper disable once InconsistentNaming
    public class CSVLoaderConfiguration : ExtentFileStorageConfiguration
    {
        public CSVSettings Settings { get; set; } = new CSVSettings();

        public override string ToString()
        {
            if (Settings != null)
            {
                return $"CSVLoaderConfiguration: {ExtentUri}, {Path}";
            }

            return base.ToString();
        }
    }
}