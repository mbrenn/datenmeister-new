using System.IO;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Models.Reports;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Reports.Adoc
{
    public class AdocEvalReportParagraph : IAdocReportEvaluator
    {
        public bool IsRelevant(IElement element)
        {
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_DatenMeister.TheOne.Reports.__ReportParagraph) == true;
        }

        public void Evaluate(AdocReportCreator adocReportCreator, IElement reportNode, TextWriter writer)
        {
            var newReportNode = adocReportCreator.GetNodeWithEvaluatedProperties(reportNode);
            
            var paragraph = newReportNode.getOrDefault<string>(_DatenMeister._Reports._ReportParagraph.paragraph);
            writer.WriteLine($"\r\n{paragraph}\r\n");
        }
    }
}