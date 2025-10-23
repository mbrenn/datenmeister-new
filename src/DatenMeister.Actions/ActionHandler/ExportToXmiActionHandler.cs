using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Actions.ActionHandler;

public class ExportToXmiActionHandler : IActionHandler
{
    private static readonly ILogger Logger = new ClassLogger(typeof(ExportToXmiActionHandler));

    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.__ExportToXmiAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        await Task.Run(() =>
        {
            var integrationSettings = actionLogic.ScopeStorage.Get<IntegrationSettings>();
            var sourcePath = action.getOrDefault<string>(_Actions._ExportToXmiAction.sourcePath);
            var filePath = action.getOrDefault<string>(_Actions._ExportToXmiAction.filePath);
            filePath = integrationSettings.NormalizeDirectoryPath(filePath);

            var workspaceId =
                action.getOrDefault<string>(_Actions._ExportToXmiAction.sourceWorkspaceId)
                ?? WorkspaceNames.WorkspaceData;

            var workspace = actionLogic.WorkspaceLogic.GetWorkspace(workspaceId);
            if (workspace == null)
            {
                var message = $"workspace is not found ${workspaceId}";
                Logger.Error(message);

                throw new InvalidOperationException(message);
            }

            var sourceElement = workspace.Resolve(sourcePath, ResolveType.NoMetaWorkspaces);
            if (sourceElement == null)
            {
                var message = $"sourcePath is not found {sourcePath}";
                Logger.Error(message);

                throw new InvalidOperationException(message);
            }

            var sourceCollection = CopyElementsActionHandler.GetCollectionFromResolvedElement(sourceElement)
                                   ?? throw new InvalidOperationException(
                                       "sourceCollection is null");

            var provider = new XmiProvider();
            var tempExtent = new MofUriExtent(provider, "dm:///export", actionLogic.ScopeStorage);

            // Now do the copying. it makes us all happy
            var extentCopier = new ExtentCopier(new MofFactory(tempExtent));
            extentCopier.Copy(sourceCollection, tempExtent.elements(), CopyOptions.CopyId);

            provider.Document.Save(filePath);
        });

        return null;
    }
}