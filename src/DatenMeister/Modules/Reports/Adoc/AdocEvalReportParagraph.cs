using System.IO;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Reports;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Reports.Adoc
{
    public class AdocEvalReportParagraph : IAdocReportEvaluator
    {
        public bool IsRelevant(IElement element)
        {
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_Reports.TheOne.__ReportParagraph) == true;
        }

        public void Evaluate(AdocReportCreator adocReportCreator, IElement reportNode, TextWriter writer)
        {
            var paragraph = reportNode.getOrDefault<string>(_Reports._ReportParagraph.paragraph);
            writer.WriteLine($"\r\n{paragraph}\r\n");
        }
    }
}