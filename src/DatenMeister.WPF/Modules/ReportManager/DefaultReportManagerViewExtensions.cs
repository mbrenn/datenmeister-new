using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BurnSystems;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.HtmlReporter.Formatter;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Windows;

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
            var listViewControl = viewExtensionInfo.GetListViewControl();
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
            }

            var itemExplorerControl = viewExtensionInfo.GetItemExplorerControl();
            if (itemExplorerControl != null)
            {
                var effectiveForm = itemExplorerControl.EffectiveForm ??
                                    throw new InvalidOperationException("EffectiveForm == null");
                
                yield return new ItemMenuButtonDefinition(
                    "Report as Html",
                    x =>
                    {
                        if (x is IExtent asExtent)
                        {
                            CreateReportForExplorerView(
                                effectiveForm,
                                asExtent.elements());
                        }
                        else
                        {
                            CreateReportForExplorerView(
                                effectiveForm,
                                new PropertiesAsReflectiveCollection(x));
                        }
                    },
                    null,
                    "Export");
            }
        }

        /// <summary>
        /// Creates the report for the currently selected element
        /// </summary>
        /// <param name="effectiveForm">The form being used for the export</param>
        /// <param name="collection">Defines the item that is selected</param>
        private void CreateReportForExplorerView(
            IObject effectiveForm,
            IReflectiveCollection collection)
        {
            var id = StringManipulation.RandomString(10);
            var tmpPath = Path.Combine(Path.GetTempPath(), id + ".html");

            using (var report = new HtmlReport(tmpPath))
            {
                report.SetDefaultCssStyle();
                
                report.StartReport("Report for Extent");
                report.Add(new HtmlHeadline("Items in collection", 1));
                var itemFormatter = new ItemFormatter(report);
                itemFormatter.FormatCollectionOfItems(collection, effectiveForm);
                report.EndReport();
            }

            Process.Start(tmpPath);
        }

        private void CreateReportForDetailElement(IObject effectiveForm, IObject selectedItem)
        {
            var id = StringManipulation.RandomString(10);
            var tmpPath = Path.Combine(Path.GetTempPath(), id + ".html");

            using (var report = new HtmlReport(tmpPath))
            {
                report.StartReport("Detail: " + selectedItem);
                report.Add(new HtmlHeadline("Detail Information", 1));
                var itemFormatter = new ItemFormatter(report);
                itemFormatter.FormatItem(selectedItem, effectiveForm);
                report.EndReport();
            }

            Process.Start(tmpPath);
        }
    }
}