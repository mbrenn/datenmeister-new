using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;

namespace DatenMeister.Core.EMOF.Implementation.Hooks.Library;

public class CompositeResolveHook : IResolveHook
{
    public object? Resolve(ResolveHookParameters hookParameters)
    {
        var mode = hookParameters.QueryString["composites"];

        var items = hookParameters.CurrentItem switch
        {
            IExtent extent => extent.elements(),
            IReflectiveCollection reflectiveCollection => reflectiveCollection,
            IObject element => new TemporaryReflectiveCollection(new[] { element }),
            _ => null
        };

        if (items is null || mode is null)
        {
            return hookParameters.CurrentItem;
        }

        switch (mode)
        {
            case "includingSelf":
                return items.GetAllCompositesIncludingThemselves();
            case "onlyComposites":
                return items.GetCompositeDescendents();
            case "allReferenced":
                return items.GetAllDescendants();
            case "allReferencedIncludingSelf":
                return items.GetAllDescendantsIncludingThemselves();
        }

        return hookParameters.CurrentItem;
    }
}