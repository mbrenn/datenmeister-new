using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Reports;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Reports.Evaluators
{
    public class HtmlReportParagraph : IHtmlReportEvaluator
    {
        public bool IsRelevant(IElement element)
        {
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_Reports.TheOne.__ReportParagraph) == true;
        }

        public void Evaluate(HtmlReportCreator htmlReportCreator, IElement reportNode)
        {
            var paragraph = reportNode.getOrDefault<string>(_Reports._ReportParagraph.paragraph);
            htmlReportCreator.HtmlReporter.Add(new HtmlParagraph(paragraph));
        }
    }
}