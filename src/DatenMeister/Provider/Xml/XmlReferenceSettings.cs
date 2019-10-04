using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Provider.Xml
{
    public class XmlReferenceSettings : ExtentLoaderConfig
    {
        public string filePath { get; set; }

        public bool keepNamespaces { get; set; }

    }
}