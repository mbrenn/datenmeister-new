using DatenMeister.Modules.Reports.Generic;

namespace DatenMeister.Modules.Reports.Adoc
{
    public class AdocEvalReportParagraph : GenericReportParagraph<AdocGenericReportCreator>
    {
        public override void WriteParagraph(AdocGenericReportCreator reportCreator, string paragraph, string cssClass)
        {
            reportCreator.TextWriter.WriteLine($"{paragraph}");
            reportCreator.TextWriter.WriteLine(string.Empty);
        }
    }
}