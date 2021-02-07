using System;
using System.IO;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.Reports;
using DatenMeister.Modules.Reports.Adoc;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Actions.ActionHandler.Reports
{
    public class AdocReportActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.@equals(
                _DatenMeister.TheOne.Actions.Reports.__AdocReportAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var reportInstance =
                action.getOrDefault<IElement>(_DatenMeister._Actions._Reports._AdocReportAction.reportInstance);
            var filePath =
                action.getOrDefault<string>(_DatenMeister._Actions._Reports._AdocReportAction.filePath);
            
            if ( string.IsNullOrEmpty(filePath))
            {
                throw new InvalidOperationException("filePath is empty");
            }

            if (reportInstance == null)
            {
                throw new InvalidOperationException("reportInstance");
            }

            using var fileStream = new StreamWriter(filePath);

            var htmlReport = new ReportLogic(
                actionLogic.WorkspaceLogic,
                actionLogic.ScopeStorage,
                new AdocReportCreator(fileStream));
            htmlReport.GenerateReportByInstance(reportInstance);
        }
    }
}