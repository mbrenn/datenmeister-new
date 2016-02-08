using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// This loader is used to store and load extents out of a file
    /// </summary>
    public class ExtentStorageConfigurationStorage : ObjectFileStorage<ExtentStorageConfigurationCollection>
    {
        private readonly List<Type> _additionalTypes = new List<Type>();

        public string Filepath { get; }
        public ExtentStorageData ExtentStorageData { get; }
        public IExtentStorageLoader ExtentLoaderLogic { get; }

        public ExtentStorageConfigurationStorage(
            ExtentStorageData extentStorageData,
            IExtentStorageLoader extentLoaderLogic,
            string filepath)
        {
            Debug.Assert(filepath != null, "filepath != null");

            ExtentLoaderLogic = extentLoaderLogic;
            ExtentStorageData = extentStorageData;
            Filepath = filepath;
        }

        public void AddAdditionalType(Type type)
        {
            _additionalTypes.Add(type);
        }

        /// <summary>
        /// Gets the additional types for the xml parsing
        /// </summary>
        /// <returns>Array of additional types</returns>
        public override Type[] GetAdditionalTypes()
        {
            return _additionalTypes.ToArray();
        }

        /// <summary>
        /// Loads all extents
        /// </summary>
        public void LoadAllExtents()
        {
            var loaded = Load(Filepath);
            if (loaded == null)
            {
                return;
            }

            foreach (var info in loaded.Extents)
            {
                try
                {
                    ExtentLoaderLogic.LoadExtent(info);
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
            ExtentLoaderLogic.StoreAll();
            var toBeStored = new ExtentStorageConfigurationCollection();
            foreach (var loadedExtent in ExtentStorageData.LoadedExtents)
            {
                toBeStored.Extents.Add(loadedExtent.Configuration);
            }

            Save(Filepath, toBeStored);
        }
    }
}