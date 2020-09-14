#nullable enable

using System;
using BurnSystems.Logging;
using DatenMeister.Integration;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Configuration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.InMemory
{
    [ConfiguredBy(typeof(InMemoryLoaderConfig))]
    public class InMemoryProviderLoader : IProviderLoader
    {
        /// <summary>
        /// Sores the logger
        /// </summary>
        private static readonly ClassLogger Logger = new ClassLogger(typeof(InMemoryLoaderConfig));

        public IWorkspaceLogic? WorkspaceLogic { get; set; }
        
        public IScopeStorage? ScopeStorage { get; set; }

        /// <summary>
        /// Just creates the provider for the memory
        /// </summary>
        /// <returns>The new InMemoryProvider</returns>
        public LoadedProviderInfo LoadProvider(ExtentLoaderConfig configuration, ExtentCreationFlags extentCreationFlags)
        {
            Logger.Info("InMemoryProvider is created");
            
            var provider = new InMemoryProvider();
            return new LoadedProviderInfo(provider);
        }

        public void StoreProvider(IProvider extent, ExtentLoaderConfig configuration)
        {
            Logger.Info("Storing of in Memory Object is not possible");
        }
    }
}