using System;
using System.IO;
using System.Threading.Tasks;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Reports;
using DatenMeister.Reports.Adoc;

namespace DatenMeister.Actions.ActionHandler.Reports
{
    public class AdocReportActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.equals(
                _DatenMeister.TheOne.Actions.Reports.__AdocReportAction) == true;
        }

        public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
        {
            await Task.Run(() =>
            {
                var reportInstance =
                    action.getOrDefault<IElement>(_DatenMeister._Actions._Reports._AdocReportAction.reportInstance);
                var filePath =
                    action.getOrDefault<string>(_DatenMeister._Actions._Reports._AdocReportAction.filePath);

                if (string.IsNullOrEmpty(filePath))
                {
                    throw new InvalidOperationException("filePath is empty");
                }

                var integrationSettings = actionLogic.ScopeStorage.Get<IntegrationSettings>();
                filePath = integrationSettings.NormalizeDirectoryPath(filePath);

                if (reportInstance == null)
                {
                    throw new InvalidOperationException("reportInstance");
                }

                var directoryPath = Path.GetDirectoryName(filePath);
                if (directoryPath != null && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using var fileStream = new StreamWriter(filePath);

                var htmlReport = new ReportLogic(
                    actionLogic.WorkspaceLogic,
                    actionLogic.ScopeStorage,
                    new AdocReportCreator(fileStream));
                htmlReport.GenerateReportByInstance(reportInstance);
            });

            return null;
        }
    }
}