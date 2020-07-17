using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.UserInteractions;

namespace DatenMeister.Modules.ExtentPropertyElementHandler
{
    public class ExtentPropertyUserInteraction : IElementInteractionsHandler
    {
        public IEnumerable<IElementInteraction> GetInteractions(IObject element)
        {   
            yield break;
        }
    }
}