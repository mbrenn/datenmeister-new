using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    public class LoadedExtentInformation
    {
        public IUriExtent Extent { get; set; }

        public ExtentStorageConfiguration Configuration { get; set; }
    }
}