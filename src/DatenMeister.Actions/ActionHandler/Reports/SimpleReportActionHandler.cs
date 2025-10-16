using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Reports.Simple;

namespace DatenMeister.Actions.ActionHandler.Reports;

/// <summary>
/// Defines the action handler for simple reports
/// </summary>
public class SimpleReportActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.Reports.__SimpleReportAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        await Task.Run(() =>
        {
            var workspace =
                action.getOrDefault<string>(_Actions._Reports._SimpleReportAction.workspaceId)
                ?? WorkspaceNames.WorkspaceData;
            var path = action.getOrDefault<string>(_Actions._Reports._SimpleReportAction.path);
            var filePath =
                action.getOrDefault<string>(_Actions._Reports._SimpleReportAction.filePath);
            var configuration =
                action.getOrDefault<IElement>(_Actions._Reports._SimpleReportAction.configuration);

            if (configuration is null)
            {
                configuration = new MofFactory(action)
                    .create(_Reports.TheOne.__SimpleReportConfiguration)
                    .SetProperty(_Reports._SimpleReportConfiguration.showDescendents, true)
                    .SetProperty(_Reports._SimpleReportConfiguration.showRootElement, true)
                    .SetProperty(_Reports._SimpleReportConfiguration.showFullName, true);
            }

            if (string.IsNullOrEmpty(filePath))
            {
                filePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "simplereport.html");
            }
            else
            {
                var integrationSettings = actionLogic.ScopeStorage.Get<IntegrationSettings>();
                filePath = integrationSettings.NormalizeDirectoryPath(filePath);
            }

            IObject? item = null;
            if (!string.IsNullOrEmpty(path))
            {
                item = ExtentHelper.TryGetItemByWorkspaceAndPath(
                    actionLogic.WorkspaceLogic,
                    workspace,
                    path);
            }

            if (item is not null)
            {
                // Set default item
                configuration.SetProperty(
                    _Reports._SimpleReportConfiguration.rootElement,
                    path);
                configuration.SetProperty(
                    _Reports._SimpleReportConfiguration.workspaceId,
                    workspace);
            }

            var simpleReport = new SimpleReportCreator(
                actionLogic.WorkspaceLogic,
                actionLogic.ScopeStorage,
                configuration);

            // Checks, if directory needs to be created
            var directory = Path.GetDirectoryName(filePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Writes the file
            using var writer = new StreamWriter(filePath);
            simpleReport.CreateReport(writer);
        });
        return null;
    }
}