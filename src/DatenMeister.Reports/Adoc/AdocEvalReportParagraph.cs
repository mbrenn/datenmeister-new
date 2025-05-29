using DatenMeister.Reports.Generic;

namespace DatenMeister.Reports.Adoc;

public class AdocEvalReportParagraph : GenericReportParagraph<AdocReportCreator>
{
    public override void WriteParagraph(AdocReportCreator reportCreator, string paragraph, string cssClass)
    {
        reportCreator.TextWriter.WriteLine($"{paragraph}");
        reportCreator.TextWriter.WriteLine(string.Empty);
    }
}