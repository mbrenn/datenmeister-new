using System;
using System.Diagnostics;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// This loader is used to store and load the extent storage out of a file.
    /// In addition, it will also use the ExtentManager class to load the actual data
    /// of the extents
    /// </summary>
    public class ExtentConfigurationLoader : ObjectFileStorage<ExtentLoaderConfigData>
    {
        /// <summary>
        /// Gets the information about the loaded extents, 
        /// and filepath where to look after
        /// </summary>
        private ExtentStorageData ExtentStorageData { get; }

        /// <summary>
        /// Gets the extent manager being used to actual load an extent
        /// </summary>
        private IExtentManager ExtentManager { get; }

        public ExtentConfigurationLoader(
            ExtentStorageData extentStorageData,
            IExtentManager extentManager)
        {
            ExtentManager = extentManager;
            ExtentStorageData = extentStorageData;
        }

        /// <summary>
        /// Adds a type for the serialization of the configuration file since the 
        /// ExtentLoaderConfig instances might be derived
        /// </summary>
        /// <param name="type"></param>
        public void AddAdditionalType(Type type)
        {
            ExtentStorageData.AdditionalTypes.Add(type);
        }

        /// <summary>
        /// Gets the additional types for the xml parsing. 
        /// This method is called by the base class to support the loading if unknown extent types
        /// </summary>
        /// <returns>Array of additional types</returns>
        public override Type[] GetAdditionalTypes()
        {
            return ExtentStorageData.AdditionalTypes.ToArray();
        }

        /// <summary>
        /// Loads all extents
        /// </summary>
        public void LoadAllExtents()
        {
            ExtentLoaderConfigData loaded = null;
            try
            {
                loaded = Load(ExtentStorageData.FilePath);
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Exception during loading of Extents: " + exc.Message);
            }

            if (loaded == null)
            {
                return;
            }

            foreach (var info in loaded.Extents)
            {
                try
                {
                    ExtentManager.LoadExtent(info, false);
                }
                catch (Exception exc)
                {
                    Debug.WriteLine($"Loading extent of {info.ExtentUri} failed: {exc.Message}");
                }
                
            }
        }

        /// <summary>
        /// Stores all extents and the catalogue of the extents
        /// </summary>
        public void StoreAllExtents()
        {
            // Stores the extents themselves into the different database
            ExtentManager.StoreAll();

            // Stores the information abotu the used extends
            var toBeStored = new ExtentLoaderConfigData();
            foreach (var loadedExtent in ExtentStorageData.LoadedExtents)
            {
                toBeStored.Extents.Add(loadedExtent.Configuration);
            }

            Save(ExtentStorageData.FilePath, toBeStored);
        }
    }
}