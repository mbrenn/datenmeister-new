using DatenMeister.Runtime.ExtentStorage.Configuration;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InconsistentNaming

namespace DatenMeister.Provider.Xml
{
    public class XmlReferenceSettings : ExtentLoaderConfig
    {
        public string? filePath { get; set; }

        public bool keepNamespaces { get; set; }

        public XmlReferenceSettings()
        {
            
        }
        
        public XmlReferenceSettings(string extentUri) : base(extentUri)
        {
        }
    }
}