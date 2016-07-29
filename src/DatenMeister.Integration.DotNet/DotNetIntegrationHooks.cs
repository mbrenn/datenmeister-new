using Autofac;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.FactoryMapper;
using DatenMeister.Web.Models;

namespace DatenMeister.Integration.DotNet
{
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