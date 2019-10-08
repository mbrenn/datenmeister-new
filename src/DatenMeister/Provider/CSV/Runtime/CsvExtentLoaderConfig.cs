using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Provider.CSV.Runtime
{
    // ReSharper disable once InconsistentNaming
    public class CsvExtentLoaderConfig : ExtentFileLoaderConfig
    {
        public CsvSettings Settings { get; set; } = new CsvSettings();

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