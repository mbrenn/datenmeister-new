using DatenMeister.Runtime.ExtentStorage.Configuration;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InconsistentNaming

namespace DatenMeister.Provider.Xml
{
    /// <summary>
    /// Loads the Xmi file and stores the result into an InMemoryObject Extent. Changes in the reference will not lead to modification
    /// of the stored and referenced file
    /// </summary>
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