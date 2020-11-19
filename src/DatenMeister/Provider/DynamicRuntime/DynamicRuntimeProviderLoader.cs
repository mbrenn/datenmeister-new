using System;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using static DatenMeister.Models._DatenMeister._DynamicRuntimeProvider;

namespace DatenMeister.Provider.DynamicRuntime
{
    public class DynamicRuntimeProviderLoader : IProviderLoader
    {
        public static readonly ILogger Logger = new ClassLogger(typeof(DynamicRuntimeProviderLoader));

        public IWorkspaceLogic? WorkspaceLogic { get; set; }

        public IScopeStorage? ScopeStorage { get;set; }

        public LoadedProviderInfo LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
        {
            var runtimeClass= configuration.getOrDefault<string>(_DynamicRuntimeLoaderConfig.runtimeClass);
            var runtimeConfiguration = configuration.getOrDefault<IElement>(_DynamicRuntimeLoaderConfig.configuration);
            if (runtimeClass == null || string.IsNullOrEmpty(runtimeClass))
            {
                Logger.Warn("No configuration is set");
            }

            var selectedType =
                AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .FirstOrDefault(x => x.FullName == runtimeClass);

            if (selectedType == null)
            {
                var message = $"Selected Type is {runtimeClass} is not found";
                throw new InvalidOperationException(message);
            }

            Logger.Info("InMemoryProvider is created");

            if (!(Activator.CreateInstance(selectedType) is IDynamicRuntimeProvider provider))
            {
                var message = $"Selected Type is {runtimeClass} is not of type IDynamicRuntimeProvider";
                throw new InvalidOperationException(message);
            }

            var resultProvider = new DynamicRuntimeProviderWrapper(provider, configuration);
            return new LoadedProviderInfo(resultProvider, configuration);
        }

        public void StoreProvider(IProvider extent, IElement configuration)
        {
            // There is no need to store this
        }
    }
}
