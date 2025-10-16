using DatenMeister.Core;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Provider.CSV.Runtime;

/// <summary>
/// The engine being used to load and store the extent into a csv file
/// </summary>
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
    public async Task<LoadedProviderInfo> LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
    {
        return await Task.Run(() =>
        {
            var dataProvider =
                new CsvLoader(WorkspaceLogic ?? throw new InvalidOperationException("WorkspaceLogic == null"));

            var provider = new InMemoryProvider();

            var filePath =
                configuration.getOrDefault<string>(_ExtentLoaderConfigs._CsvExtentLoaderConfig.filePath);
            if (filePath == null || string.IsNullOrEmpty(filePath))
            {
                throw new InvalidOperationException("FilePath is empty");
            }

            var doesFileExist = File.Exists(filePath);
            if (doesFileExist)
            {
                var settings =
                    configuration.getOrDefault<IElement>(_ExtentLoaderConfigs._CsvExtentLoaderConfig
                        .settings) ?? throw new InvalidOperationException("Settings are not set");

                dataProvider.Load(
                    provider,
                    filePath,
                    settings);
            }
            else if (extentCreationFlags == ExtentCreationFlags.LoadOnly)
            {
                throw new InvalidOperationException(
                    $"File '{filePath}' does not exist and creation of extents is not granted via extentCreationFlags. Real Path: " +
                    $"{Path.Combine(Environment.CurrentDirectory, configuration.getOrDefault<string>(_ExtentLoaderConfigs._CsvExtentLoaderConfig.filePath))}");
            }

            return new LoadedProviderInfo(provider);
        });
    }

    public async Task StoreProvider(IProvider extent, IElement configuration)
    {
        await Task.Run(() =>
        {
            var filePath = configuration.getOrDefault<string>(_ExtentLoaderConfigs._CsvExtentLoaderConfig.filePath);

            if (filePath == null)
                throw new InvalidOperationException("csvConfiguration.filePath == null");

            var provider = new CsvLoader(WorkspaceLogic ?? throw new InvalidOperationException("WorkspaceLogic == null"));

            var settings = configuration.getOrDefault<IElement>(_ExtentLoaderConfigs._CsvExtentLoaderConfig.settings);

            provider.Save(extent, filePath, settings);
        });
    }

    public ProviderLoaderCapabilities ProviderLoaderCapabilities { get; } = new()
    {
        IsPersistant = true,
        AreChangesPersistant = false
    };
}