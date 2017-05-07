using System;
using System.IO;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.CSV.Runtime
{
    /// <summary>
    /// The engine being used to load and store the extent into a csv file
    /// </summary>
    [ConfiguredBy(typeof(CSVExtentLoaderConfig))]
    // ReSharper disable once InconsistentNaming
    public class CSVExtentLoader : IExtentLoader
    {
        private readonly IWorkspaceLogic _workspaceLogic;

        public CSVExtentLoader(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        public IProvider LoadExtent(ExtentLoaderConfig configuration, bool createAlsoEmpty)
        {
            var csvConfiguration = (CSVExtentLoaderConfig) configuration;
            var dataProvider = new CSVLoader(_workspaceLogic);

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

        public void StoreExtent(IProvider extent, ExtentLoaderConfig configuration)
        {
            var csvConfiguration = (CSVExtentLoaderConfig) configuration;

            var provider = new CSVLoader(_workspaceLogic);
            provider.Save(extent, csvConfiguration.Path, csvConfiguration.Settings);
        }
    }
}