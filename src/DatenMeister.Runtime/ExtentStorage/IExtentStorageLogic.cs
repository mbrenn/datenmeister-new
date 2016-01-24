using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    public interface IExtentStorageLogic
    {
        /// <summary>
        /// Loads the extent by using the extent storage
        /// </summary>
        /// <param name="configuration">Configuration being used to load</param>
        /// <returns>The loaded extent</returns>
        IUriExtent LoadExtent(ExtentStorageConfiguration configuration);

        /// <summary>
        /// Stores the extent according to the used configuration during loading. 
        /// If loading was not performed, an exception is thrown. 
        /// </summary>
        /// <param name="extent"></param>
        void StoreExtent(IUriExtent extent);

        void StoreAll();
    }
}