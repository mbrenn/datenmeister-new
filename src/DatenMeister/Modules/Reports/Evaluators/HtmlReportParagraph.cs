using System.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Reports;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Modules.TextTemplates;
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
            // Checks, if we have an evalParagraph
            if (reportNode.isSet(_Reports._ReportParagraph.evalParagraph))
            {
                var viewNode = reportNode.getOrDefault<IElement>(_Reports._ReportParagraph.viewNode);
                var evalParagraph = reportNode.getOrDefault<string>(_Reports._ReportParagraph.evalParagraph);
                if (viewNode == null)
                {
                    htmlReportCreator.HtmlReporter.Add(new HtmlParagraph("No Viewnode found"));
                    return;
                }

                var dataViewEvaluation = htmlReportCreator.GetDataViewEvaluation();
                var element =
                    dataViewEvaluation.GetElementsForViewNode(viewNode).OfType<IElement>().FirstOrDefault();
                if (element == null)
                {
                    htmlReportCreator.HtmlReporter.Add(new HtmlParagraph("No Element found"));
                    return;
                }

                var evalResult = TextTemplateEngine.Parse(element, evalParagraph);
                htmlReportCreator.HtmlReporter.Add(new HtmlParagraph(evalResult));
            }
            else
            {
                var paragraph = reportNode.getOrDefault<string>(_Reports._ReportParagraph.paragraph);
                htmlReportCreator.HtmlReporter.Add(new HtmlParagraph(paragraph));
            }
        }
    }
}