using DatenMeister.Modules.HtmlExporter.HtmlEngine;
using DatenMeister.Modules.Reports.Generic;

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