using DatenMeister.Modules.Reports.Generic;

namespace DatenMeister.Modules.Reports.Adoc
{
    public class AdocEvalReportHeadline : GenericReportHeadline<AdocReportCreator>
    {
        public override void WriteHeadline(AdocReportCreator reportCreator, string headline)
        {
            reportCreator.TextWriter.WriteLine($"== {headline}");
        }
    }
}