using DatenMeister.EMOF.Interface.Identifiers;

namespace DatenMeister.Runtime.ExtentStorage.Interfaces
{
    /// <summary>
    /// Defines the interface to store an extent persistently whether in a database or
    /// in the file. 
    /// The application can use this implementation to load and store a file on startup
    /// and end of application
    /// </summary>
    public interface IExtentStorage
    {
        /// <summary>
        /// Loads the extent according to the given configuration
        /// </summary>
        /// <param name="configuration">Configuration to be used to retrive the information</param>
        /// <returns>Loaded extent</returns>
        IUriExtent LoadExtent(ExtentStorageConfiguration configuration);

        /// <summary>
        /// Sores the extent according to the given configuration
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        /// <param name="configuration">Configuration to be added</param>
        void StoreExtent(IUriExtent extent, ExtentStorageConfiguration configuration);
    }
}