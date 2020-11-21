using System.Collections.Generic;
using System.Globalization;
using DatenMeister.Runtime;
using DatenMeister.Core.EMOF.Interface.Reflection;
using static DatenMeister.Models._DatenMeister._DynamicRuntimeProvider;

namespace DatenMeister.Provider.DynamicRuntime
{
    /// <summary>
    /// Wraps the dynamic runtime provider so it is usable for the DatenMeister
    /// </summary>
    public class DynamicRuntimeProviderWrapper : IProvider
    {
        private readonly IDynamicRuntimeProvider _provider;

        private readonly IElement _configuration;

        /// <summary>
        /// Initializes a new instance of the DynamicRuntimeProviderWrapper
        /// </summary>
        /// <param name="provider">Provider to be used</param>
        /// <param name="configuration"></param>
        public DynamicRuntimeProviderWrapper(
            IDynamicRuntimeProvider provider,
            IElement configuration)
        {
            _provider = provider;
            _configuration = configuration;
        }

        public IProviderObject CreateElement(string? metaClassUri)
        {
            throw new System.NotImplementedException();
        }

        public void AddElement(IProviderObject? valueAsObject, int index = -1)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteElement(string id)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAllElements()
        {
            throw new System.NotImplementedException();
        }

        public IProviderObject? Get(string? id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<IProviderObject> GetRootObjects()
        {

            var runtimeConfiguration = _configuration.getOrDefault<IElement>(_DynamicRuntimeLoaderConfig.configuration);
            var n = 1;
            foreach (var element in _provider.Get(this, runtimeConfiguration))
            {
                if (string.IsNullOrEmpty(element.Id))
                {
                    element.Id = n.ToString(CultureInfo.InvariantCulture);
                }

                yield return element;
                n++;
            }
        }

        public ProviderCapability GetCapabilities()
        {
            return new ProviderCapability
            {
                CanCreateElements = false,
                IsTemporaryStorage = false
            };
        }
    }
}