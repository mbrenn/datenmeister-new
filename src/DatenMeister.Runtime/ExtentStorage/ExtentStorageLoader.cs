using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// This logic handles the loading and storing of extents automatically. 
    /// This loader is responsible to retrieve an extent by the given ExtentStorageConfiguration
    /// and storing it afterwards at the same location
    /// </summary>
    public class ExtentStorageLoader : IExtentStorageLoader
    {
        private readonly ExtentStorageData _data;

        /// <summary>
        /// Stores the mapping
        /// </summary>
        private readonly IConfigurationToExtentStorageMapper _map;

        /// <summary>
        /// Stores the access to the workspaces
        /// </summary>
        private readonly IWorkspaceCollection _workspaceCollection;

        private readonly ILifetimeScope _diScope;

        public ExtentStorageLoader(ExtentStorageData data, IConfigurationToExtentStorageMapper map)
        {
            Debug.Assert(map != null, "map != null");
            Debug.Assert(data != null, "data != null");

            _data = data;
            _map = map;
        }

        public ExtentStorageLoader(
            ExtentStorageData data, 
            IConfigurationToExtentStorageMapper map,
            IWorkspaceCollection workspaceCollection,
            ILifetimeScope diScope
            ) : this(data, map)
        {
            Debug.Assert(workspaceCollection != null, "collection != null");
            _workspaceCollection = workspaceCollection;
            _diScope = diScope;
        }

        /// <summary>
        /// Loads the extent by using the extent storage by using the configuration and finding
        /// the correct storage engine 
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
            var extentStorage = _map.CreateFor(_diScope, configuration);
        
            // Loads the extent
            var loadedExtent = extentStorage.LoadExtent(configuration, createAlsoEmpty);
            Debug.WriteLine($"Loading extent: {configuration}");

            if (loadedExtent == null)
            {
                throw new InvalidOperationException("Extent for configuration could not be loaded");
            }

            AddToWorkspaceIfPossible(configuration, loadedExtent);

            // Stores the information into the data container
            var info = new ExtentStorageData.LoadedExtentInformation
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

            Debug.WriteLine($"Writing extent: {information.Configuration}");

            var extentStorage = _map.CreateFor(_diScope, information.Configuration);
            extentStorage.StoreExtent(information.Extent, information.Configuration);
        }

        public void DetachExtent(IUriExtent extent)
        {
            lock (_data.LoadedExtents)
            {
                var information = _data.LoadedExtents.FirstOrDefault(x => x.Extent == extent);
                if (information != null)
                {
                    _data.LoadedExtents.Remove(information);
                    Debug.WriteLine($"Detaching extent: {information.Configuration}");
                }
            }
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