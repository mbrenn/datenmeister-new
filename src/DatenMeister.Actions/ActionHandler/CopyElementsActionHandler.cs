using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Actions.ActionHandler;

public class CopyElementsActionHandler : IActionHandler
{
    private static readonly ILogger Logger = new ClassLogger(typeof(CopyElementsActionHandler));

    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.__CopyElementsAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        await Task.Run(() =>
        {
            var sourceWorkspaceId =
                action.getOrDefault<string>(_Actions._CopyElementsAction.sourceWorkspace)
                ?? WorkspaceNames.WorkspaceData;
            var targetWorkspaceId =
                action.getOrDefault<string>(_Actions._CopyElementsAction.targetWorkspace)
                ?? WorkspaceNames.WorkspaceData;
            var sourcePath =
                action.getOrDefault<string>(_Actions._CopyElementsAction.sourcePath);
            var targetPath =
                action.getOrDefault<string>(_Actions._CopyElementsAction.targetPath);
            var moveOnly =
                action.getOrDefault<bool>(_Actions._CopyElementsAction.moveOnly);
            var emptyTarget =
                action.getOrDefault<bool>(_Actions._CopyElementsAction.emptyTarget);

            var sourceWorkspace = actionLogic.WorkspaceLogic.GetWorkspace(sourceWorkspaceId);
            var targetWorkspace = actionLogic.WorkspaceLogic.GetWorkspace(targetWorkspaceId);
            if (sourceWorkspace == null)
            {
                var message = $"sourceWorkspace is not found {sourceWorkspaceId}";
                Logger.Error(message);

                throw new InvalidOperationException(message);
            }

            if (targetWorkspace == null)
            {
                var message = $"targetWorkspace is not found {targetWorkspaceId}";
                Logger.Error(message);

                throw new InvalidOperationException(message);
            }

            var sourceElement = sourceWorkspace.Resolve(sourcePath, ResolveType.NoMetaWorkspaces);
            var targetElement = targetWorkspace.Resolve(targetPath, ResolveType.NoMetaWorkspaces);

            if (sourceElement == null)
            {
                var message = $"sourcePath is not found ${sourcePath}";
                Logger.Error(message);

                throw new InvalidOperationException(message);
            }

            if (targetElement == null)
            {
                var message = $"targetPath is not found ${targetPath}";
                Logger.Error(message);

                throw new InvalidOperationException(message);
            }

            // Depending on type, get the reflective instance
            var targetCollection = GetCollectionFromResolvedElement(targetElement)
                                   ?? throw new InvalidOperationException(
                                       "targetElement is null");

            if (emptyTarget)
            {
                targetCollection.clear();
            }

            if (sourceElement is IElement asElement && !(sourceElement is IExtent))
            {
                // Now do the copying. it makes us all happy
                targetCollection.add(
                    ObjectCopier.Copy(new MofFactory(targetCollection),
                        asElement,
                        CopyOptions.CopyId));

                if (moveOnly)
                {
                    Logger.Warn("Moving of single elements is not supported since it is not known" +
                                "how the element is connected to its container. ");
                }
            }
            else
            {
                var sourceCollection = GetCollectionFromResolvedElement(sourceElement)
                                       ?? throw new InvalidOperationException(
                                           "sourceCollection is null");

                // Now do the copying. it makes us all happy
                var extentCopier = new ExtentCopier(new MofFactory(targetCollection));
                extentCopier.Copy(sourceCollection, targetCollection, CopyOptions.CopyId);

                if (moveOnly)
                {
                    sourceCollection.clear();
                }
            }
        });

        return null;
    }

    public static IReflectiveCollection? GetCollectionFromResolvedElement(object sourceElement)
    {
        IReflectiveCollection? sourceCollection = null;
        if (sourceElement is IExtent sourceExtent)
        {
            sourceCollection = sourceExtent.elements();
        }
        else if (sourceElement is IReflectiveCollection asSourceCollection)
        {
            sourceCollection = asSourceCollection;
        }
        else if (sourceElement is IElement asElement)
        {
            sourceCollection =
                asElement.get<IReflectiveCollection>(
                    DefaultClassifierHints.GetDefaultPackagePropertyName(asElement));
        }

        return sourceCollection;
    }
}