#nullable enable 
using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Provider.InMemory
{
    public class InMemoryLoaderConfig : ExtentLoaderConfig
    {
        public InMemoryLoaderConfig()
        {
            
        }
        
        public InMemoryLoaderConfig(string extentUri) : base(extentUri)
        {
        }
    }
}