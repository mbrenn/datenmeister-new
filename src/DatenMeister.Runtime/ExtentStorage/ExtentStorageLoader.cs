using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// This logic handles the loading and storing of extents automatically
    /// </summary>
    public class ExtentStorageLoader : IExtentStorageLoader
    {
        private readonly ExtentStorageData _data;

        /// <summary>
        /// Stores the mapping
        /// </summary>
        private readonly IConfigurationToExtentStorageMapper _map;

        private readonly IWorkspaceCollection _workspaceCollection;

        public ExtentStorageLoader(ExtentStorageData data, IConfigurationToExtentStorageMapper map)
        {
            Debug.Assert(map != null, "map != null");
            Debug.Assert(data != null, "data != null");

            _data = data;
            _map = map;
        }

        public ExtentStorageLoader(ExtentStorageData data, IConfigurationToExtentStorageMapper map, IWorkspaceCollection workspaceCollection)
        {
            Debug.Assert(map != null, "map != null");
            Debug.Assert(workspaceCollection != null, "collection != null");
            Debug.Assert(data != null, "data != null");

            _data = data;
            _map = map;
            _workspaceCollection = workspaceCollection;
        }

        /// <summary>
        /// Loads the extent by using the extent storage
        /// </summary>
        /// <param name="configuration">Configuration being used to load</param>
        /// <returns>The loaded extent</returns>
        public IUriExtent LoadExtent(ExtentStorageConfiguration configuration)
        {
            var extentStorage = _map.CreateFor(configuration);
        
            // Loads the extent
            var loadedExtent = extentStorage.LoadExtent(configuration);
            Debug.WriteLine($"- Loading: {configuration}");

            if (loadedExtent == null)
            {
                throw new InvalidOperationException("Extent for configuration could not be loaded");
            }

            AddToWorkspaceIfPossible(configuration, loadedExtent);

            // Stores the information into the data container
            var info = new ExtentStorageData.LoadedExtentInformation()
            {
                Configuration = configuration,
                Extent = loadedExtent
            };

            lock (_data.LoadedExtents)
            {
                _data.LoadedExtents.Add(info);
            }

            return loadedExtent;
        }

        private void AddToWorkspaceIfPossible(ExtentStorageConfiguration configuration, IUriExtent loadedExtent)
        {
            if (_workspaceCollection != null)
            {
                var workspace = _workspaceCollection.GetWorkspace(configuration.Workspace);
                if (workspace == null)
                {
                    throw new InvalidOperationException($"Workspace {configuration.Workspace} not found");
                }

                workspace.AddExtentNoDuplicate(loadedExtent);
            }
        }

        /// <summary>
        /// Stores the extent according to the used configuration during loading. 
        /// If loading was not performed, an exception is thrown. 
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        public void StoreExtent(IUriExtent extent)
        {
            ExtentStorageData.LoadedExtentInformation information;
            lock (_data.LoadedExtents)
            {
                information = _data.LoadedExtents.FirstOrDefault(x => x.Extent == extent);
            }

            if (information == null)
            {
                throw new InvalidOperationException($"The extent '{extent}' was not loaded by this instance");
            }

            Debug.WriteLine($"- Writing: {information.Configuration}");

            var extentStorage = _map.CreateFor(information.Configuration);
            extentStorage.StoreExtent(extent, information.Configuration);
        }

        public void StoreAll()
        {
            Debug.WriteLine("Writing all extents");

            List<ExtentStorageData.LoadedExtentInformation> copy;
            lock (_data.LoadedExtents)
            {
                copy = _data.LoadedExtents.ToList();
            }

            foreach (var info in copy)
            {
                StoreExtent(info.Extent);
            }
        }
    }
}