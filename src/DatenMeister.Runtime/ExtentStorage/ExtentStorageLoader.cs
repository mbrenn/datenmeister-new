using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DatenMeister.DataLayer;
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

        private readonly IDataLayerLogic _dataLayerLogic;

        private readonly IWorkspaceCollection _workspaceCollection;

        public ExtentStorageLoader(ExtentStorageData data, IConfigurationToExtentStorageMapper map, IDataLayerLogic dataLayerLogic)
        {
            Debug.Assert(map != null, "map != null");
            Debug.Assert(data != null, "data != null");
            Debug.Assert(dataLayerLogic != null, "dataLayerLogic != null");

            _data = data;
            _map = map;
            _dataLayerLogic = dataLayerLogic;
        }

        public ExtentStorageLoader(
            ExtentStorageData data, 
            IConfigurationToExtentStorageMapper map,
            IDataLayerLogic dataLayerLogic,
            IWorkspaceCollection workspaceCollection
            ) : this(data, map, dataLayerLogic)
        {
            Debug.Assert(workspaceCollection != null, "collection != null");
            _workspaceCollection = workspaceCollection;
        }

        /// <summary>
        /// Loads the extent by using the extent storage
        /// </summary>
        /// <param name="configuration">Configuration being used to load</param>
        /// <param name="createAlsoEmpty">true, if also empty extents will be created, if the file does not exist</param>
        /// <returns>The loaded extent</returns>
        public IUriExtent LoadExtent(ExtentStorageConfiguration configuration, bool createAlsoEmpty = false)
        {
            // Check, if the extent url is a real uri
            if (!Uri.IsWellFormedUriString(configuration.ExtentUri, UriKind.Absolute))
            {
                throw new InvalidOperationException($"Uri is not well-formed: {configuration.ExtentUri}");
            }

            // Creates the extent storage, being capable to load or store an extent
            var extentStorage = _map.CreateFor(configuration);
        
            // Loads the extent
            var loadedExtent = extentStorage.LoadExtent(_dataLayerLogic, configuration, createAlsoEmpty);
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
            extentStorage.StoreExtent(_dataLayerLogic, information.Extent, information.Configuration);
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