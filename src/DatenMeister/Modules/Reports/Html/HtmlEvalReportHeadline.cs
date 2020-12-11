using DatenMeister.Modules.HtmlExporter.HtmlEngine;
using DatenMeister.Modules.Reports.Generic;

namespace DatenMeister.Modules.Reports.Html
{
    public class HtmlEvalReportHeadline : GenericReportHeadline<HtmlReportCreator>
    {
        public override void WriteHeadline(HtmlReportCreator reportCreator, string headline)
        {
            reportCreator.HtmlReporter.Add(new HtmlHeadline(headline, 1));
        }
    }
}