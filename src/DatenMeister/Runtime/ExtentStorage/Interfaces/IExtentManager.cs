using System;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage.Configuration;

namespace DatenMeister.Runtime.ExtentStorage.Interfaces
{
    public interface IExtentManager
    {
        /// <summary>
        /// Loads the extent by using the extent storage
        /// </summary>
        /// <param name="configuration">Configuration being used to load</param>
        /// <param name="createAlsoEmpty">true, if also empty extents will be created, if the file does not exist</param>
        /// <returns>The loaded extent</returns>
        IUriExtent LoadExtent(ExtentLoaderConfig configuration, bool createAlsoEmpty);

        /// <summary>
        /// Stores the extent according to the used configuration during loading. 
        /// If loading was not performed, an exception is thrown. 
        /// </summary>
        /// <param name="extent"></param>
        void StoreExtent(IExtent extent);

        /// <summary>
        /// Detaches a specific extent in a way that is not known to the storage loader anymore
        /// </summary>
        /// <param name="extent">Extent to be detached</param>
        void DetachExtent(IExtent extent);

        /// <summary>
        /// Deletes the extent from the extent manager and also workspace. The extent manager will 
        /// </summary>
        /// <param name="extent">Extent to be deleted</param>
        void DeleteExtent(IExtent extent);

        /// <summary>
        /// Loads all extents
        /// </summary>
        void LoadAllExtents();

        /// <summary>
        /// Stores all extents according configuration
        /// </summary>
        void StoreAllExtents();

        /// <summary>
        /// Gets the loading configuration for the given extent or null, if 
        /// the extent does not contain a configuration
        /// </summary>
        /// <param name="extent">The extent whose configuration is retrieved</param>
        /// <returns>The configuration</returns>
        ExtentLoaderConfig GetLoadConfigurationFor(IUriExtent extent);

        /// <summary>
        /// Creates the storage type definitions as defined within the storage types
        /// </summary>
        void CreateStorageTypeDefinitions();
    }
}