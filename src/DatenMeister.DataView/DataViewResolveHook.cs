using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.Hooks;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;

namespace DatenMeister.DataView;

/// <summary>
/// Implements a resolve hook that allows resolving elements from a data view
/// </summary>
public class DataViewResolveHook : IResolveHook
{
    /// <summary>
    /// Stores the logger
    /// </summary>
    private static readonly ILogger logger = new ClassLogger(typeof(DataViewResolveHook));

    /// <summary>
    /// Resolves the given query string and returns the result
    /// </summary>
    /// <param name="hookParameters">The parameters for the resolve hook</param>
    /// <returns>The resolved object or the original item if not resolved</returns>
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