namespace DatenMeister.Runtime.Extents
{
    public class ImportSettings
    {
        public string fileToBeImported { get; set; } = string.Empty;
        public string newExtentUri { get; set; } = string.Empty;
        public string fileToBeExported { get; set; } = string.Empty;
        public string Workspace { get; set; } = string.Empty;
    }
}