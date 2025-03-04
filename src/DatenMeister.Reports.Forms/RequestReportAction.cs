﻿using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.HtmlEngine;
using DatenMeister.Reports.Forms.Model;
using DatenMeister.Reports.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Reports.Forms
{
    internal class RequestReportAction : IActionHandler
    {
        private readonly IWorkspaceLogic workspaceLogic;
        private readonly IScopeStorage scopeStorage;

        public RequestReportAction(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            this.workspaceLogic = workspaceLogic;
            this.scopeStorage = scopeStorage;
        }

        public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
        {
            var workspace = action.getOrDefault<string>(_Root._RequestReportAction.workspace);
            var itemUri = action.getOrDefault<string>(_Root._RequestReportAction.itemUri);

            var textWriter = new StringWriter();
            var reportLogic = new ReportLogic(workspaceLogic, scopeStorage, new HtmlReportCreator(textWriter)
            {
                EmbedInExistingPage = true
            });

            // Generates the report
            var foundReportInstance = workspaceLogic.FindElement(workspace, itemUri)
                ?? throw new InvalidOperationException($"Report was not found: {workspace} / {itemUri}");

            reportLogic.GenerateReportByInstance(foundReportInstance);

            // Returns the found report
            var result = InMemoryObject.CreateEmpty(_Root.TheOne.__RequestReportResult);
            result.set(_Root._RequestReportResult.report, textWriter.ToString());

            return await Task.FromResult(result);
        }

        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.equals(_Root.TheOne.@__RequestReportAction) == true;
        }
    }
}
