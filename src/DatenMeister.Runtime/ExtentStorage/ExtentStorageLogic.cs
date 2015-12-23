using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// This logic handles the loading and storing of extents automatically
    /// </summary>
    public class ExtentStorageLogic
    {
        /// <summary>
        /// Stores the loaded extents 
        /// </summary>
        private readonly List<LoadedExtentInformation> _loadedExtents = new List<LoadedExtentInformation>();

        private readonly IExtentStorageToConfigurationMap _map;

        public ExtentStorageLogic(IExtentStorageToConfigurationMap map)
        {
            Debug.Assert(map != null, "map != null");
            _map = map;
        }

        public IUriExtent Load(ExtentStorageConfiguration configuration)
        {
            var extentStorage = _map.CreateFor(configuration);

            var loadedExtent = extentStorage.LoadExtent(configuration);
            if (loadedExtent == null)
            {
                throw new InvalidOperationException("Extent for configuration coudd not be loaded");
            }

            var info = new LoadedExtentInformation()
            {
                Configuration = configuration,
                Extent = loadedExtent
            };

            lock (_loadedExtents)
            {
                _loadedExtents.Add(info);
            }

            return loadedExtent;
        }

        /// <summary>
        /// Stores the extent according to the used configuration during loading. 
        /// If loading was not performed, an exception is thrown. 
        /// </summary>
        /// <param name="extent"></param>
        public void Store(IUriExtent extent)
        {
            LoadedExtentInformation information;
            lock (_loadedExtents)
            {
                information = _loadedExtents.FirstOrDefault(x => x.Extent == extent);
            }

            if (information == null)
            {
                throw new InvalidOperationException($"The extent '{extent}was not loaded by this instance");
            }
            
            var extentStorage = _map.CreateFor(information.Configuration);
            extentStorage.StoreExtent(extent, information.Configuration);
        }

        public void StoreAll()
        {
            List<LoadedExtentInformation> copy;
            lock (_loadedExtents)
            {
                copy = _loadedExtents.ToList();
            }

            foreach (var info in copy)
            {
                Store(info.Extent);
            }
        }
    }
}