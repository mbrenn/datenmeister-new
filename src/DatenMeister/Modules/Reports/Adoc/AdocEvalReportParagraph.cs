using System.Collections.Generic;
using System.IO;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.TextTemplates;
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
            
            // Evaluates the paragraph if required
            if (reportNode.isSet(_DatenMeister._Reports._ReportParagraph.evalParagraph))
            {
                // Dynamic evaluation
                if (adocReportCreator.GetDataEvaluation(reportNode, out var element) || element == null) return;

                var evalParagraph = reportNode.getOrDefault<string>(_DatenMeister._Reports._ReportParagraph.evalParagraph);
                paragraph = TextTemplateEngine.Parse(
                    evalParagraph,
                    new Dictionary<string, object> {["i"] = element});
            }
            
            writer.WriteLine($"\r\n{paragraph}\r\n");
        }
    }
}