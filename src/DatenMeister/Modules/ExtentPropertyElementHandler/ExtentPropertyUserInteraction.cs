using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.ManagementProvider;
using DatenMeister.Models.Runtime;
using DatenMeister.Modules.UserInteractions;

namespace DatenMeister.Modules.ExtentPropertyElementHandler
{
    public class ExtentPropertyUserInteraction : IElementInteractionsHandler
    {
        private readonly IScopeStorage _scopeStorage;

        public ExtentPropertyUserInteraction(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }

        public IEnumerable<IElementInteraction> GetInteractions(IObject element)
        {
            var extentSettings = _scopeStorage.Get<ExtentSettings>();
            foreach (var property in extentSettings.propertyDefinitions)
            {
                
            }
            
            yield break;
        }
    }
}