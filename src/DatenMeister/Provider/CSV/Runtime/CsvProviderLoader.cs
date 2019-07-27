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
    public class CsvProviderLoader : IProviderLoader
    {
        private readonly IWorkspaceLogic _workspaceLogic;

        public CsvProviderLoader(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, ExtentCreationFlags extentCreationFlags)
        {
            var csvConfiguration = (CSVExtentLoaderConfig) configuration;
            var dataProvider = new CSVLoader(_workspaceLogic);

            var provider = new InMemoryProvider();

            var doesFileExist = File.Exists(csvConfiguration.filePath);
            if (doesFileExist)
            {
                dataProvider.Load(provider, csvConfiguration.filePath, csvConfiguration.Settings);
            }
            else if (extentCreationFlags == ExtentCreationFlags.LoadOnly)
            {
                throw new InvalidOperationException(
                    $"File '{csvConfiguration.filePath}' does not exist and creation of extents is not granted via extentCreationFlags. Real Path: {Path.Combine(Environment.CurrentDirectory, csvConfiguration.filePath)}");
            }

            return new LoadedProviderInfo(provider);
        }

        public void StoreProvider(IProvider extent, ExtentLoaderConfig configuration)
        {
            var csvConfiguration = (CSVExtentLoaderConfig) configuration;

            var provider = new CSVLoader(_workspaceLogic);
            provider.Save(extent, csvConfiguration.filePath, csvConfiguration.Settings);
        }
    }
}