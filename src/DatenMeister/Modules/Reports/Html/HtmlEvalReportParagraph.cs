using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.HtmlExporter.HtmlEngine;
using DatenMeister.Modules.Reports.Generic;
using DatenMeister.Modules.TextTemplates;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Reports.Html
{
    public class HtmlReportParagraph : GenericReportParagraph<HtmlReportCreator>
    {
        public override void WriteParagraph(HtmlReportCreator report, string paragraph, string cssClass)
        {
            var htmlParagraph = new HtmlParagraph(paragraph);

            // Gets the cssClass
            if (!string.IsNullOrEmpty(cssClass) && cssClass != null)
            {
                htmlParagraph.CssClass = cssClass;
            }

            // Creates the paragraph
            report.HtmlReporter.Add(htmlParagraph);
        }
    }
}