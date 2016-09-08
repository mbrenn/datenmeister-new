using System;
using System.Diagnostics;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// This loader is used to store and load extents out of a file
    /// </summary>
    public class ExtentStorageConfigurationLoader : ObjectFileStorage<ExtentStorageConfigurationCollection>
    {
        private string Filepath { get; }
        private ExtentStorageData ExtentStorageData { get; }
        private IExtentStorageLoader ExtentLoaderLogic { get; }

        public ExtentStorageConfigurationLoader(
            ExtentStorageData extentStorageData,
            IExtentStorageLoader extentLoaderLogic,
            string filepath)
        {
            Debug.Assert(filepath != null, "filepath != null");

            ExtentLoaderLogic = extentLoaderLogic;
            ExtentStorageData = extentStorageData;
            Filepath = filepath;
        }

        /// <summary>
        /// Adds a type for the serialization of the configuration file since the 
        /// ExtentStorageConfiguration instances might be derived
        /// </summary>
        /// <param name="type"></param>
        public void AddAdditionalType(Type type)
        {
            ExtentStorageData.AdditionalTypes.Add(type);
        }

        /// <summary>
        /// Gets the additional types for the xml parsing
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
            ExtentStorageConfigurationCollection loaded = null;
            try
            {
                loaded = Load(Filepath);
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
                    ExtentLoaderLogic.LoadExtent(info, false);
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
            ExtentLoaderLogic.StoreAll();

            // Stores the information abotu the used extends
            var toBeStored = new ExtentStorageConfigurationCollection();
            foreach (var loadedExtent in ExtentStorageData.LoadedExtents)
            {
                toBeStored.Extents.Add(loadedExtent.Configuration);
            }

            Save(Filepath, toBeStored);
        }
    }
}