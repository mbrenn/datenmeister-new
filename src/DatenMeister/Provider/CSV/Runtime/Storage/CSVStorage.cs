using System;
using System.IO;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.CSV.Runtime.Storage
{
    /// <summary>
    /// The engine being used to load and store the extent into a csv file
    /// </summary>
    [ConfiguredBy(typeof(CSVStorageConfiguration))]
    // ReSharper disable once InconsistentNaming
    public class CSVStorage : IExtentStorage
    {
        private readonly IWorkspaceLogic _workspaceLogic;

        public CSVStorage(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        public IProvider LoadExtent(ExtentStorageConfiguration configuration, bool createAlsoEmpty)
        {
            var csvConfiguration = (CSVStorageConfiguration) configuration;
            var dataProvider = new CSVDataProvider(_workspaceLogic);

            var provider = new InMemoryProvider();

            var doesFileExist = File.Exists(csvConfiguration.Path);
            if (doesFileExist)
            {
                dataProvider.Load(provider, csvConfiguration.Path, csvConfiguration.Settings);
            }
            else if (!createAlsoEmpty)
            {
                throw new InvalidOperationException(
                    $"File does not exist and empty extents is not given in argument {nameof(createAlsoEmpty)}");
            }

            return provider;
        }

        public void StoreExtent(IProvider extent, ExtentStorageConfiguration configuration)
        {
            var csvConfiguration = (CSVStorageConfiguration) configuration;

            var provider = new CSVDataProvider(_workspaceLogic);
            provider.Save(extent, csvConfiguration.Path, csvConfiguration.Settings);
        }
    }
}