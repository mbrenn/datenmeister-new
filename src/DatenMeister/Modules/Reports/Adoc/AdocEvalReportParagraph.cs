using System.Collections.Generic;
using System.IO;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.Reports.Generic;
using DatenMeister.Modules.TextTemplates;
using DatenMeister.Runtime;

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