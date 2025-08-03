using DatenMeister.Reports.Generic;

namespace DatenMeister.Reports.Adoc;

public class AdocEvalReportHeadline : GenericReportHeadline<AdocReportCreator>
{
    public override void WriteHeadline(AdocReportCreator reportCreator, string headline)
    {
        reportCreator.TextWriter.WriteLine($"== {headline}");
    }
}