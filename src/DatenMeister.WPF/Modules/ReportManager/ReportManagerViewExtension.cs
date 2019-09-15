using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using BurnSystems;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.HtmlReporter.Formatter;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
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
            if (viewExtensionTargetInformation.NavigationHost is DetailFormWindow
            && viewExtensionTargetInformation.NavigationGuest is DetailFormControl detailFormControl)
            {
                yield return 
                    new ItemMenuButtonDefinition(
                        "As Html",
                        (x) => CreateReportForDetailElement(detailFormControl, x),
                        null,
                        "Report"
                    );
            }

            if (viewExtensionTargetInformation.NavigationGuest is ItemListViewControl viewControl)
            {
                yield return new CollectionMenuButtonDefinition(
                    "As Html",
                    (x) => CreateReportForExplorerView(viewControl, x),
                    null,
                    "Item.Report")
                {
                    IsTopCategoryFixed = true
                };
            }
        }

        /// <summary>
        /// Creates the report for the currently selected element 
        /// </summary>
        /// <param name="explorerControl">The explorer control being used</param>
        /// <param name="selectedItem">Defines the item that is selected</param>
        private void CreateReportForExplorerView(
            ItemListViewControl explorerControl, 
            IReflectiveCollection collection)
        {
            var form = explorerControl.EffectiveForm;
            var id = StringManipulation.RandomString(10);
            var tmpPath = Path.Combine(Path.GetTempPath(), id + ".html");
            
            using (var report = new HtmlReport(tmpPath))
            {
                report.StartReport("List");
                report.Add(new HtmlHeadline("Items in collection", 1));
                var itemFormatter = new ItemFormatter(report);
                itemFormatter.FormatCollectionOfItems(collection, form);
                report.EndReport();
            }

            Process.Start(tmpPath);
        }

        private void CreateReportForDetailElement(DetailFormControl detailFormControl, IObject selectedItem)
        {
            var id = StringManipulation.RandomString(10);
            var tmpPath = Path.Combine(Path.GetTempPath(), id + ".html");
            
            using (var report = new HtmlReport(tmpPath))
            {
                report.StartReport("Detail: " + selectedItem);
                report.Add(new HtmlHeadline("Detail Information", 1));
                var itemFormatter = new ItemFormatter(report);
                itemFormatter.FormatItem(selectedItem, detailFormControl.EffectiveForm);
                report.EndReport();
            }

            Process.Start(tmpPath);
        }
    }
}