using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Provider.CSV.Runtime
{
    // ReSharper disable once InconsistentNaming
    public class CsvExtentLoaderConfig : ExtentFileLoaderConfig
    {
        public CsvSettings settings { get; set; } = new CsvSettings();

        public override string ToString()
        {
            if (settings != null)
            {
                return $"CSVExtentLoaderConfig: {extentUri}, {filePath}";
            }

            return base.ToString();
        }

        public CsvExtentLoaderConfig()
        {
            
        }

        public CsvExtentLoaderConfig(string extentUri) : base(extentUri)
        {
        }
    }
}