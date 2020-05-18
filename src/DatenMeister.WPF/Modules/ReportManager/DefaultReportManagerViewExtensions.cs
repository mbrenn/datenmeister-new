using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using BurnSystems;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.HtmlReporter.Formatter;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Modules.Reports;
using DatenMeister.Runtime;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Information;

namespace DatenMeister.WPF.Modules.ReportManager
{
    public class DefaultReportManagerViewExtensions : IViewExtensionFactory
    {
        public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionInfo viewExtensionInfo)
        {
            // Check if the current query is about the detail form
            var detailFormControl = viewExtensionInfo.GetDetailFormControlOfDetailWindow();
            if (detailFormControl != null)
            {
                var effectiveForm = detailFormControl.EffectiveForm ??
                                    throw new InvalidOperationException("effectiveForm == null");
                yield return
                    new ItemMenuButtonDefinition(
                        "As Html",
                        (x) => CreateReportForDetailElement(effectiveForm, x),
                        null,
                        "Item"
                    );
            }

            // Check if the the query is about the current view
            /*var listViewControl = viewExtensionInfo.GetListViewControl();
            if (listViewControl != null)
            {
                var effectiveForm = listViewControl.EffectiveForm ??
                                    throw new InvalidOperationException("effectiveForm == null");
                yield return new CollectionMenuButtonDefinition(
                    "As Html",
                    (x) => CreateReportForExplorerView(effectiveForm, x),
                    null,
                    "Collection")
                {
                    IsTopCategoryFixed = true
                };
            }*/

            var itemExplorerControl = viewExtensionInfo.GetItemExplorerControl();
            if (itemExplorerControl != null)
            {
                yield return new ItemMenuButtonDefinition(
                    "Report as Html",
                    x =>
                    {
                        if (x is IExtent asExtent)
                        {
                            CreateReportForExplorerView(asExtent);
                        }
                        else
                        {
                            CreateReportForExplorerView(x);
                        }
                    },
                    null,
                    "Export");
            }
        }

        /// <summary>
        /// Creates the report for the currently selected element
        /// </summary>
        /// <param name="rootElement">Defines the item that is selected</param>
        private void CreateReportForExplorerView(IObject rootElement)
        {
            var reportConfiguration = new SimpleReportConfiguration
            {
                form = null, 
                rootElement = rootElement,
                showDescendents = true,
                showRootElement = true,
                showFullName = true
            };

            var id = StringManipulation.RandomString(10);
            var tmpPath = Path.Combine(Path.GetTempPath(), id + ".html");
            using var streamWriter = new StreamWriter(tmpPath, false, Encoding.UTF8);

            
            var reportCreator = new ReportCreator(GiveMe.Scope.WorkspaceLogic, reportConfiguration);
            reportCreator.CreateReport(streamWriter);

            DotNetHelper.CreateProcess(tmpPath);
        }

        private void CreateReportForDetailElement(IObject effectiveForm, IObject selectedItem)
        {
            var id = StringManipulation.RandomString(10);
            var tmpPath = Path.Combine(Path.GetTempPath(), id + ".html");

            using (var report = new HtmlReport(tmpPath))
            {
                report.StartReport("Detail: " + selectedItem);
                report.Add(new HtmlHeadline("Detail Information", 1));
                var itemFormatter = new ItemFormatter(report, GiveMe.Scope.WorkspaceLogic);
                itemFormatter.FormatItem(selectedItem, effectiveForm);
                report.EndReport();
            }

            DotNetHelper.CreateProcess(tmpPath);
        }
    }
}