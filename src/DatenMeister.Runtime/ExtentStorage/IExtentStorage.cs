using System.Runtime.InteropServices.ComTypes;
using DatenMeister.EMOF.Interface.Identifiers;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// Defines the interface to store an extent persistently whether in a database or
    /// in the file. 
    /// The application can use this implementation to load and store a file on startup
    /// and end of application
    /// </summary>
    public interface IExtentStorage<in T> where T : ExtentStorageConfiguration
    {
        /// <summary>
        /// Loads the extent according to the given configuration
        /// </summary>
        /// <param name="configuration">Configuration to be used to retrive the information</param>
        /// <returns>Loaded extent</returns>
        IUriExtent LoadExtent(T configuration);

        /// <summary>
        /// Sores the extent according to the given configuration
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        /// <param name="configuration">Configuration to be used</param>
        void StoreExtent(IUriExtent extent, T configuration);
    }
}