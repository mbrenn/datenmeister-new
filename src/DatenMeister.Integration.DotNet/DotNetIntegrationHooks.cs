using Autofac;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.FactoryMapper;

namespace DatenMeister.Integration.DotNet
{
    /// <summary>
    /// These integration hooks are called by the integration
    /// when the DotNetIntegration shall be performed.
    /// </summary>
    public class DotNetIntegrationHooks : IIntegrationHooks
    {
        public void OnStartScope(ILifetimeScope scope)
        {
            var defaultFactoryMapper = scope.Resolve<IFactoryMapper>() as DefaultFactoryMapper;
            defaultFactoryMapper?.PerformAutomaticMappingByAttribute();

            var storageMap = scope.Resolve<IConfigurationToExtentStorageMapper>() as ManualConfigurationToExtentStorageMapper;
            storageMap?.PerformMappingForConfigurationOfExtentLoaders();
        }

        public void BeforeLoadExtents(ILifetimeScope scope)
        {
            // Now start the plugins  
            LoadingHelper.StartPlugins(scope);
        }
    }
}