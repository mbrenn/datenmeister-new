using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Provider.XMI.ExtentStorage
{
    /// <summary>
    /// Loads the given .xmi file and stores it into the extent as direct reference to the datenmeister
    /// 
    /// </summary>
    public class XmiStorageConfiguration : ExtentFileLoaderConfig
    {
        public XmiStorageConfiguration()
        {
            
        }
        
        public XmiStorageConfiguration(string extentUri) : base(extentUri)
        {
        }
    }
}