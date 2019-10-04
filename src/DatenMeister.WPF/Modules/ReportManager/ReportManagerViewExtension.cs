using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using BurnSystems;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.HtmlReporter.Formatter;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Lists;
using DatenMeister.WPF.Windows;

namespace DatenMeister.WPF.Modules.ReportManager
{
    public class ReportManagerViewExtension : IViewExtensionFactory
    {
        public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            // Check if the current query is about the detail form
            if (viewExtensionTargetInformation.NavigationHost is DetailFormWindow
            && viewExtensionTargetInformation.NavigationGuest is DetailFormControl detailFormControl)
            {
                yield return
                    new ItemMenuButtonDefinition(
                        "As Html",
                        (x) => CreateReportForDetailElement(detailFormControl.EffectiveForm, x),
                        null,
                        "Item"
                    );
            }

            // Check fi the the query is about the current view
            if (viewExtensionTargetInformation.NavigationGuest is ItemListViewControl viewControl)
            {
                yield return new CollectionMenuButtonDefinition(
                    "As Html",
                    (x) => CreateReportForExplorerView(viewControl.EffectiveForm, x),
                    null,
                    "Collection")
                {
                    IsTopCategoryFixed = true
                };
            }

            if (viewExtensionTargetInformation.NavigationGuest is ItemExplorerControl explorerControl)
            {
                yield return new ItemMenuButtonDefinition(
                    "Report as Html",
                    x=>
                    {
                        if (x is IExtent asExtent)
                        {
                            CreateReportForExplorerView(
                                explorerControl.EffectiveForm,
                                explorerControl.Items ?? asExtent.elements());
                        }
                        else
                        {
                            CreateReportForExplorerView(
                                explorerControl.EffectiveForm,
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
                report.StartReport("List");
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