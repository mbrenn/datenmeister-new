﻿using System;
using System.IO;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Integration;
using DatenMeister.Modules.Reports;
using DatenMeister.Modules.Reports.Html;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Actions.ActionHandler.Reports
{
    public class HtmlReportActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.@equals(
                _DatenMeister.TheOne.Actions.Reports.__HtmlReportAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var reportInstance =
                action.getOrDefault<IElement>(_DatenMeister._Actions._Reports._HtmlReportAction.reportInstance);
            var filePath =
                action.getOrDefault<string>(_DatenMeister._Actions._Reports._HtmlReportAction.filePath);

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
                new HtmlReportCreator(fileStream));
            htmlReport.GenerateReportByInstance(reportInstance);
        }
    }
}