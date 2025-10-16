using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;

namespace DatenMeister.Core.EMOF.Implementation.Hooks.Library;

public class FilterMetaClassResolveHook : IResolveHook
{
    public object? Resolve(ResolveHookParameters hookParameters)
    {
        var metaClass = hookParameters.QueryString["metaclass"];
            
        var items = hookParameters.CurrentItem switch
        {
            IExtent extent => extent.elements(),
            IReflectiveCollection reflectiveCollection => reflectiveCollection, 
            _ => null
        };

        if (items is null || metaClass is null)
        {
            return hookParameters.CurrentItem;
        }

        return items.WhenMetaClassIs(new MofObjectShadow(metaClass));
    }
}