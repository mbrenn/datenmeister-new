using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Integration;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    /// <summary>
    /// This logic handles the loading and storing of extents automatically. 
    /// This loader is responsible to retrieve an extent by the given ExtentLoaderConfig
    /// and storing it afterwards at the same location
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ExtentManager : IExtentManager
    {
        private readonly ExtentStorageData _data;

        /// <summary>
        /// Stores the mapping between configuration types and storage provider
        /// </summary>
        private readonly IConfigurationToExtentStorageMapper _map;

        private readonly ILifetimeScope _diScope;

        private readonly IWorkspaceLogic _workspaceLogic;

        private readonly IntegrationSettings _integrationSettings;

        public ExtentManager(
            ExtentStorageData data, 
            IConfigurationToExtentStorageMapper map,
            ILifetimeScope diScope, 
            IWorkspaceLogic workspaceLogic,
            IntegrationSettings integrationSettings)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _workspaceLogic = workspaceLogic ?? throw new ArgumentNullException(nameof(workspaceLogic));
            _integrationSettings = integrationSettings ?? throw new ArgumentNullException(nameof(integrationSettings));
            _diScope = diScope;
        }

        /// <summary>
        /// Loads the extent by using the extent storage by using the configuration and finding
        /// the correct storage engine 
        /// </summary>
        /// <param name="configuration">Configuration being used to load</param>
        /// <param name="createAlsoEmpty">true, if also empty extents will be created, if the file does not exist</param>
        /// <returns>The loaded extent</returns>
        public IUriExtent LoadExtent(ExtentLoaderConfig configuration, bool createAlsoEmpty = false)
        {
            // Check, if the extent url is a real uri
            if (!Uri.IsWellFormedUriString(configuration.ExtentUri, UriKind.Absolute))
            {
                throw new InvalidOperationException($"Uri is not well-formed: {configuration.ExtentUri}");
            }

            // Checks, if the given URL has a relative path
            if (configuration is ExtentFileLoaderConfig fileConfiguration)
            {
                if (!Path.IsPathRooted(fileConfiguration.Path))
                {
                    fileConfiguration.Path = Path.Combine(_integrationSettings.DatabasePath, fileConfiguration.Path);
                }
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

            var uriExtent = new MofUriExtent(loadedExtent, configuration.ExtentUri);
            AddToWorkspaceIfPossible(configuration, uriExtent);

            // Stores the information into the data container
            var info = new ExtentStorageData.LoadedExtentInformation
            {
                Configuration = configuration,
                Extent = uriExtent
            };

            lock (_data.LoadedExtents)
            {
                _data.LoadedExtents.Add(info);
            }

            return uriExtent;
        }

        /// <summary>
        /// Adds the given extent to the workspace according to the ExtentLoaderConfig. If the
        /// workspace does not exist, it will be created
        /// </summary>
        /// <param name="configuration">Configuration to be used</param>
        /// <param name="loadedExtent">The loaded extent to be added to the workpace</param>
        private void AddToWorkspaceIfPossible(ExtentLoaderConfig configuration, IUriExtent loadedExtent)
        {
            if (_workspaceLogic != null)
            {
                var workspace = string.IsNullOrEmpty(configuration.Workspace)
                    ? _workspaceLogic.GetDefaultWorkspace()
                    : _workspaceLogic.GetWorkspace(configuration.Workspace);

                if (workspace == null)
                {
                    throw new InvalidOperationException($"Workspace {configuration.Workspace} not found");
                }

                workspace.AddExtentNoDuplicate(_workspaceLogic, loadedExtent);
            }
        }

        /// <summary>
        /// Gets the loading configuration for the given extent or null, if 
        /// the extent does not contain a configuration
        /// </summary>
        /// <param name="extent">The extent whose configuration is retrieved</param>
        /// <returns>The configuration</returns>
        public ExtentLoaderConfig GetLoadConfigurationFor(IUriExtent extent)
        {
            ExtentStorageData.LoadedExtentInformation information;
            lock (_data.LoadedExtents)
            {
                information = _data.LoadedExtents.FirstOrDefault(x => x.Extent == extent);
            }

            return information?.Configuration;
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
            extentStorage.StoreExtent(((MofUriExtent) information.Extent).Provider, information.Configuration);
        }

        /// <summary>
        /// Detaches the extent by removing it from the database of loaded extents
        /// </summary>
        /// <param name="extent"></param>
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