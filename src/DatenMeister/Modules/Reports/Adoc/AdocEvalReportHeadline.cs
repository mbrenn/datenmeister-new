using System.IO;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.Reports.Generic;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Reports.Adoc
{
    public class AdocEvalReportHeadline : GenericReportHeadline<AdocGenericReportCreator>
    {
        public void Evaluate(AdocGenericReportCreator adocGenericReportCreator, IElement reportNode, TextWriter writer)
        {
            var title = reportNode.getOrDefault<string>(_DatenMeister._Reports._Elements._ReportHeadline.title);
            writer.WriteLine($"== {title}");
        }

        public override void WriteHeadline(AdocGenericReportCreator reportCreator, string headline)
        {
            reportCreator.TextWriter.WriteLine($"== {headline}");
        }
    }
}