using System;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;
using static DatenMeister.Core.Models._DatenMeister._DynamicRuntimeProvider;

namespace DatenMeister.Provider.DynamicRuntime
{
    public class DynamicRuntimeProviderLoader : IProviderLoader
    {
        public static readonly ILogger Logger = new ClassLogger(typeof(DynamicRuntimeProviderLoader));

        public IWorkspaceLogic? WorkspaceLogic { get; set; }

        public IScopeStorage? ScopeStorage { get;set; }

        public LoadedProviderInfo LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
        {
            var runtimeClass = configuration.getOrDefault<string>(_DynamicRuntimeLoaderConfig.runtimeClass);
            var provider = CreateInstanceByRuntimeClass<IDynamicRuntimeProvider>(runtimeClass);

            var resultProvider = new DynamicRuntimeProviderWrapper(
                provider,
                configuration);
            return new LoadedProviderInfo(resultProvider, configuration);
        }

        public static T CreateInstanceByRuntimeClass<T>(string runtimeClass) where T : class
        {
            if (string.IsNullOrEmpty(runtimeClass))
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
                var message = $"Selected .Net-Type to create {runtimeClass} is not found";
                throw new InvalidOperationException(message);
            }

            Logger.Info($"InMemoryProvider {runtimeClass} is created");

            if (!(Activator.CreateInstance(selectedType) is T result))
            {
                var message = $"Selected Type is {runtimeClass} is not of type IDynamicRuntimeProvider";
                throw new InvalidOperationException(message);
            }

            return result;
        }

        public void StoreProvider(IProvider extent, IElement configuration)
        {
            // There is no need to store this
        }
    }
}
