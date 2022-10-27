#nullable enable

using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Core.Provider.InMemory
{
    public class InMemoryProviderLoader : IProviderLoader
    {
        /// <summary>
        /// Sores the logger
        /// </summary>
        private static readonly ClassLogger Logger = new(typeof(InMemoryProviderLoader));

        public IWorkspaceLogic? WorkspaceLogic { get; set; }

        public IScopeStorage? ScopeStorage { get; set; }

        /// <summary>
        /// Just creates the provider for the memory
        /// </summary>
        /// <returns>The new InMemoryProvider</returns>
        public LoadedProviderInfo LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
        {
            Logger.Info("InMemoryProvider is created");

            var provider = new InMemoryProvider();
            return new LoadedProviderInfo(provider);
        }

        public void StoreProvider(IProvider extent, IElement configuration)
        {
            Logger.Info("Storing of in Memory Object is not possible");
        }

        public ProviderLoaderCapabilities ProviderLoaderCapabilities { get; } =
            new()
            {
                IsPersistant = false,
                AreChangesPersistant = false
            };
    }
}