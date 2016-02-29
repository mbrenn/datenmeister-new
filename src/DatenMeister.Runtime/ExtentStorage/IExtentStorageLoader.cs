using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    public interface IExtentStorageLoader
    {
        /// <summary>
        /// Loads the extent by using the extent storage
        /// </summary>
        /// <param name="configuration">Configuration being used to load</param>
        /// <param name="createAlsoEmpty">true, if also empty extents will be created, if the file does not exist</param>
        /// <returns>The loaded extent</returns>
        IUriExtent LoadExtent(ExtentStorageConfiguration configuration, bool createAlsoEmpty);

        /// <summary>
        /// Stores the extent according to the used configuration during loading. 
        /// If loading was not performed, an exception is thrown. 
        /// </summary>
        /// <param name="extent"></param>
        void StoreExtent(IUriExtent extent);

        void StoreAll();
    }
}