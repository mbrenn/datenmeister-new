using BurnSystems.Logging;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Provider.DynamicRuntime;

namespace DatenMeister.Actions.Transformations;

/// <summary>
/// Defines the transformation class for the item transformation
/// </summary>
public class ItemTransformationActionHandler : IActionHandler
{
    private static readonly ILogger Logger = new ClassLogger(typeof(ItemTransformationActionHandler));
        
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.__TransformItemsAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        await Task.Run(() =>
        {
            var metaClass = action.getOrDefault<IElement>(_Actions._TransformItemsAction.metaClass);
            var runtimeClass =
                action.getOrDefault<string>(_Actions._TransformItemsAction.runtimeClass);
            var workspace =
                action.getOrDefault<string>(_Actions._TransformItemsAction.workspaceId);
            var path = action.getOrDefault<string>(_Actions._TransformItemsAction.path);

            var sourceWorkspace = actionLogic.WorkspaceLogic.GetWorkspace(workspace);
            if (sourceWorkspace == null)
            {
                var message = $"sourceWorkspace is not found {workspace}";
                Logger.Error(message);

                throw new InvalidOperationException(message);
            }

            var sourceElement = sourceWorkspace.Resolve(path, ResolveType.IncludeWorkspace);

            if (sourceElement == null)
            {
                var message = $"sourcePath is not found ${path}";
                Logger.Error(message);

                throw new InvalidOperationException(message);
            }

            // Depending on type, get the reflective instance
            var targetCollection = CopyElementsActionHandler.GetCollectionFromResolvedElement(sourceElement)
                                   ?? throw new InvalidOperationException(
                                       "targetElement is null");

            var transformer =
                DynamicRuntimeProviderLoader.CreateInstanceByRuntimeClass<IItemTransformation>(runtimeClass);

            foreach (var item in targetCollection.GetAllCompositesIncludingThemselves())
            {
                if (!(item is IElement asElement)) continue;
                if (metaClass != null)
                {
                    if (asElement.metaclass?.Equals(metaClass) != true)
                    {
                        continue;
                    }
                }

                // Transform item
                transformer.TransformItem(asElement, action);
            }
        });

        return null;
    }
}