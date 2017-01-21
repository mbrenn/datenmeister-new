using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Provider;
using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Runtime.ExtentStorage.Interfaces
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

        /// <summary>
        /// Detaches a specific extent in a way that is not known to the storage loader anymore
        /// </summary>
        /// <param name="extent">Extent to be detached</param>
        void DetachExtent(IUriExtent extent);

        void StoreAll();
    }
}