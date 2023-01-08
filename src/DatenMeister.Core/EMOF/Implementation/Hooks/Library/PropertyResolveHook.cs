using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.Helper;

namespace DatenMeister.Core.EMOF.Implementation.Hooks.Library
{
    public class PropertyResolveHook : IResolveHook
    {
        public object? Resolve(ResolveHookParameters hookParameters)
        {
            // Checks whether we have a property
            var property = hookParameters.QueryString.Get("prop");
            if (property == null)
            {
                return hookParameters.CurrentItem;
            }
            
            if (hookParameters.CurrentItem is MofElement mofElement)
            {
                return mofElement.get<IReflectiveCollection>(property);
            }

            return hookParameters.CurrentItem;
        }
    }
}