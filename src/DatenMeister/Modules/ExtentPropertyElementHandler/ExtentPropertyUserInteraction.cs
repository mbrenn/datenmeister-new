using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.UserInteractions;

namespace DatenMeister.Modules.ExtentPropertyElementHandler
{
    public class ExtentPropertyUserInteraction : IElementInteractionsHandler
    {
        private IScopeStorage _scopeStorage;

        public ExtentPropertyUserInteraction(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }

        public IEnumerable<IElementInteraction> GetInteractions(IObject element)
        {   
            yield break;
        }
    }
}