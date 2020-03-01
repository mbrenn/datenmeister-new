using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Provider.XMI.ExtentStorage
{
    public class XmiStorageConfiguration : ExtentFileLoaderConfig
    {
        public XmiStorageConfiguration(string extentUri) : base(extentUri)
        {
        }
    }
}