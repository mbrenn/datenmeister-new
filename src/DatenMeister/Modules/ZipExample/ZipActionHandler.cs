using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.UserInteractions;

namespace DatenMeister.Modules.ZipExample
{
    public class ZipActionHandler :  BaseElementInteractionHandler
    {
        public override IEnumerable<IElementInteraction> GetInteractions(IElement element)
        {
            if (IsRelevant(element))
            {
                
            }
            throw new System.NotImplementedException();
        }
    }
}