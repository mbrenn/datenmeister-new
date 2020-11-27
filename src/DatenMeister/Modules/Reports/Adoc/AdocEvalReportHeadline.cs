using System.IO;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.Reports.Generic;
using DatenMeister.Runtime;

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