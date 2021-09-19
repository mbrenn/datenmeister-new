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

            // Unfortunately, due to the reflection structure of the .Net Core and unit tests
            // There might be multiple assemblies loaded in which one lives on a complete 'shadow'
            // structure with its own inheritance tree. This means that we will try to instantiiate
            // every instance until we found the correct one
            var createdType =
                AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => x.FullName == runtimeClass)
                    .Select(Activator.CreateInstance)
                    .FirstOrDefault(x => x is T);
            
            if (createdType is null)
            {
                var message = $"Selected instantiation of type {runtimeClass} is null";
                throw new InvalidOperationException(message);
            }
            if (createdType is not T result)
            {
                var message = $"Selected Type is {runtimeClass} is not of type {typeof(T).FullName}." +
                              $"It is of type {createdType.GetType().FullName}";
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
