using System;
using System.IO;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.CSV.Runtime.Storage
{
    /// <summary>
    /// The engine being used to load and store the extent into a csv file
    /// </summary>
    [ConfiguredBy(typeof(CSVStorageConfiguration))]
    // ReSharper disable once InconsistentNaming
    public class CSVStorage : IExtentStorage
    {
        private readonly IWorkspaceCollection _workspaceCollection;
        private readonly IDataLayerLogic _dataLayerLogic;

        public CSVStorage(IWorkspaceCollection workspaceCollection, IDataLayerLogic dataLayerLogic)
        {
            _workspaceCollection = workspaceCollection;
            _dataLayerLogic = dataLayerLogic;
        }

        public IUriExtent LoadExtent(ExtentStorageConfiguration configuration, bool createAlsoEmpty)
        {
            var csvConfiguration = (CSVStorageConfiguration) configuration;
            var provider = new CSVDataProvider(_workspaceCollection, _dataLayerLogic); 
            var mofExtent = new MofUriExtent(csvConfiguration.ExtentUri);
            var factory = new MofFactory();

            var doesFileExist = File.Exists(csvConfiguration.Path);
            if (doesFileExist)
            {
                provider.Load(mofExtent, factory, csvConfiguration.Path, csvConfiguration.Settings);
            }
            else if (!createAlsoEmpty)
            {
                throw new InvalidOperationException(
                    $"File does not exist and empty extents is not given in argument {nameof(createAlsoEmpty)}");
            }

            return mofExtent;
        }

        public void StoreExtent(IUriExtent extent, ExtentStorageConfiguration configuration)
        {
            var csvConfiguration = (CSVStorageConfiguration) configuration;

            var provider = new CSVDataProvider(_workspaceCollection, _dataLayerLogic);
            provider.Save(extent, csvConfiguration.Path, csvConfiguration.Settings);
        }
    }
}