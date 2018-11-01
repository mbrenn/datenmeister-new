using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Provider.CSV.Runtime
{
    // ReSharper disable once InconsistentNaming
    public class CSVExtentLoaderConfig : ExtentFileLoaderConfig
    {
        public CSVSettings Settings { get; set; } = new CSVSettings();

        public override string ToString()
        {
            if (Settings != null)
            {
                return $"CSVExtentLoaderConfig: {extentUri}, {filePath}";
            }

            return base.ToString();
        }
    }
}