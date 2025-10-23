using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.Hooks;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;

namespace DatenMeister.DataView;

public class DataViewResolveHook : IResolveHook
{
    private static readonly ILogger logger = new ClassLogger(typeof(DataViewResolveHook));

    public object? Resolve(ResolveHookParameters hookParameters)
    {
        // Now check whether we have a dataview
        var dataview = hookParameters.QueryString.Get("dataview");
        var scopeStorage = hookParameters.ScopeStorage;
        if (dataview != null && hookParameters.CurrentItem is IReflectiveCollection reflectiveCollection)
        {
            if (hookParameters.Extent is not IUriResolver uriResolver)
            {
                throw new InvalidOperationException("Given extent does not support IUriResolver interface");
            }
                
            if (scopeStorage == null)
            {
                logger.Error("Dataview queried but extent does not have Scope Storage set");
                throw new InvalidOperationException("Dataview queried but extent does not have Scope Storage set");
            }

            var dataviewElement = uriResolver.ResolveElement(dataview, ResolveType.Default);
            if (dataviewElement == null)
            {
                logger.Warn($"Dataview was not found: {dataview}");
            }
            else
            {
                var dataViewEvaluation = new DataViewEvaluation(scopeStorage.Get<DataViewNodeFactories>());
                dataViewEvaluation.AddDynamicSource("input", reflectiveCollection);
                return dataViewEvaluation.GetElementsForViewNode(dataviewElement);
            }
        }

        return hookParameters.CurrentItem;
    }
}