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
    [ConfiguredBy(typeof(CsvExtentLoaderConfig))]
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
            var csvConfiguration = (CsvExtentLoaderConfig) configuration;
            var dataProvider = new CsvLoader(_workspaceLogic);

            var provider = new InMemoryProvider();

            var filePath = csvConfiguration.filePath;
            if (filePath == null || string.IsNullOrEmpty(filePath))
            {
                throw new InvalidOperationException("FilePath is empty");    
            }
            
            
            var doesFileExist = File.Exists(filePath);
            if (doesFileExist)
            {
                dataProvider.Load(provider, filePath, csvConfiguration.Settings);
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
            var csvConfiguration = (CsvExtentLoaderConfig) configuration;

            if (csvConfiguration.filePath == null)
                throw new InvalidOperationException("csvConfiguration.filePath == null");
            
            var provider = new CsvLoader(_workspaceLogic);
            provider.Save(extent, csvConfiguration.filePath, csvConfiguration.Settings);
        }
    }
}