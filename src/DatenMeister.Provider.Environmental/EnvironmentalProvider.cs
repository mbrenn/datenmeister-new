using System;
using System.Collections;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Provider.Environmental
{
    /// <summary>
    /// Gets the environmental provider
    /// </summary>
    public class EnvironmentalProvider : IProviderLoader
    {
        public IWorkspaceLogic? WorkspaceLogic { get; set; }
        public IScopeStorage? ScopeStorage { get; set; }
        
        public LoadedProviderInfo LoadProvider(IElement configuration, ExtentCreationFlags extentCreationFlags)
        {
            var provider = new InMemoryProvider();
            
            var variables = Environment.GetEnvironmentVariables();
            foreach (var pair in variables.OfType<DictionaryEntry>().OrderBy(x => x.Key))
            {
                var value = new InMemoryObject(provider, _DatenMeister.TheOne.CommonTypes.OSIntegration.__EnvironmentalVariable.Uri);
                value.SetProperty(_DatenMeister._CommonTypes._OSIntegration._EnvironmentalVariable.name, pair.Key);
                value.SetProperty(_DatenMeister._CommonTypes._OSIntegration._EnvironmentalVariable.value, pair.Value);
                value.Id = pair.Key.ToString();
                provider.AddElement(value);
            }

            return new LoadedProviderInfo(provider);
        }

        /// <summary>
        /// No storage possible
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="configuration"></param>
        public void StoreProvider(IProvider extent, IElement configuration)
        {
        }

        public ProviderLoaderCapabilities ProviderLoaderCapabilities { get; } = new ProviderLoaderCapabilities()
        {
            IsPersistant = false,
            AreChangesPersistant = false
        };
    }
}