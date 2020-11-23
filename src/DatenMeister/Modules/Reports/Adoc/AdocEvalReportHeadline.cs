using System.IO;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.Reports.Generic;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Reports.Adoc
{
    public class AdocEvalReportHeadline :GenericReportHeadline, IAdocReportEvaluator
    {
        public void Evaluate(AdocReportCreator adocReportCreator, IElement reportNode, TextWriter writer)
        {
            var title = reportNode.getOrDefault<string>(_DatenMeister._Reports._ReportHeadline.title);
            writer.WriteLine($"== {title}");
        }
    }
}