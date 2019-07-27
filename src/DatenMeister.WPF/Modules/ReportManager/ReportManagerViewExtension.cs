using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using BurnSystems;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.HtmlReporter.Formatter;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
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
                    new RibbonButtonDefinition(
                        "As Html",
                        () => CreateReportForDetailElement(detailFormControl),
                        null,
                        "Report"
                    );
            }
        }

        private void CreateReportForDetailElement(DetailFormControl detailFormControl)
        {
            var id = StringManipulation.RandomString(10);
            var tmpPath = Path.Combine(Path.GetTempPath(), id + ".html");
            
            using (var report = new HtmlReport(tmpPath))
            {
                report.StartReport("Detail: " + detailFormControl.DetailElement);
                report.Add(new HtmlHeadline("Detail Information", 1));
                var itemFormatter = new ItemFormatter(report);
                itemFormatter.FormatItem(detailFormControl.DetailElement, detailFormControl.EffectiveForm);
                report.EndReport();
            }

            Process.Start(tmpPath);
        }
    }
}