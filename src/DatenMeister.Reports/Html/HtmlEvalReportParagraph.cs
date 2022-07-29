using DatenMeister.HtmlEngine;
using DatenMeister.Reports.Generic;

namespace DatenMeister.Reports.Html
{
    public class HtmlReportParagraph : GenericReportParagraph<HtmlReportCreator>
    {
        public override void WriteParagraph(HtmlReportCreator report, string paragraph, string cssClass)
        {
            var htmlParagraph = new HtmlParagraph(paragraph);

            // Gets the cssClass
            if (!string.IsNullOrEmpty(cssClass))
            {
                htmlParagraph.CssClass = cssClass;
            }

            // Creates the paragraph
            report.HtmlReporter.Add(htmlParagraph);
        }
    }
}