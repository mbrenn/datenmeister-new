using System;
using System.IO;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
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
        public IWorkspaceLogic? WorkspaceLogic { get; set; }
        public IScopeStorage? ScopeStorage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration">CsvExtentLoaderConfig</param>
        /// <param name="extentCreationFlags"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public LoadedProviderInfo LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
        {
            var dataProvider = new CsvLoader(WorkspaceLogic ?? throw new InvalidOperationException("WorkspaceLogic == null"));

            var provider = new InMemoryProvider();

            var filePath = configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.filePath);
            if (filePath == null || string.IsNullOrEmpty(filePath))
            {
                throw new InvalidOperationException("FilePath is empty");    
            }
            
            throw new NotImplementedException();
            /*
            var doesFileExist = File.Exists(filePath);
            if (doesFileExist)
            {
                dataProvider.Load(provider, filePath, csvConfiguration.settings);
            }
            else if (extentCreationFlags == ExtentCreationFlags.LoadOnly)
            {
                throw new InvalidOperationException(
                    $"File '{filePath}' does not exist and creation of extents is not granted via extentCreationFlags. Real Path: {Path.Combine(Environment.CurrentDirectory, csvConfiguration.filePath)}");
            }

            return new LoadedProviderInfo(provider);
            */
        }

        public void StoreProvider(IProvider extent, IElement configuration)
        {
            var filePath = configuration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._CsvExtentLoaderConfig.filePath);

            if (filePath == null)
                throw new InvalidOperationException("csvConfiguration.filePath == null");
            
            var provider = new CsvLoader(WorkspaceLogic ?? throw new InvalidOperationException("WorkspaceLogic == null"));
            
            throw new NotImplementedException();
            // provider.Save(extent, filePath, csvConfiguration.settings);
        }
    }
}