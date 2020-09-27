using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Reports;
using DatenMeister.Modules.HtmlExporter.HtmlEngine;
using DatenMeister.Modules.TextTemplates;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;

namespace DatenMeister.Modules.Reports.Html
{
    public class HtmlReportParagraph : IHtmlReportEvaluator
    {
        public bool IsRelevant(IElement element)
        {
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_Reports.TheOne.__ReportParagraph) == true;
        }

        public void Evaluate(HtmlReportCreator htmlReportCreator, IElement reportNodeOrigin)
        {
            var reportNode = ObjectCopier.CopyForTemporary(reportNodeOrigin);
            if (reportNode.isSet(_Reports._ReportParagraph.evalProperties))
            {
                GetDataEvaluation(out var element);
                var evalProperties = reportNode.getOrDefault<string>(_Reports._ReportParagraph.evalProperties);

                var dict = new Dictionary<string, object> {["v"] = reportNode};
                if (element != null)
                {
                    dict["i"] = element;
                }
                
                TextTemplateEngine.Parse(
                    "{{" + evalProperties + "}}",
                    dict);
            }

            HtmlParagraph htmlParagraph;

            // Gets the htmlParagraph
            if (reportNode.isSet(_Reports._ReportParagraph.evalParagraph))
            {
                // Dynamic evaluation
                if (GetDataEvaluation(out var element) || element == null) return;

                var evalParagraph = reportNode.getOrDefault<string>(_Reports._ReportParagraph.evalParagraph);
                var evalResult = TextTemplateEngine.Parse(
                    evalParagraph,
                    new Dictionary<string, object> {["i"] = element});
                htmlParagraph = new HtmlParagraph(evalResult);
            }
            else
            {
                var paragraph = reportNode.getOrDefault<string>(_Reports._ReportParagraph.paragraph);
                htmlParagraph = new HtmlParagraph(paragraph);
            }
            
            // Gets the cssClass
            var cssClass = reportNode.getOrDefault<string>(_Reports._ReportParagraph.cssClass);
            if (!string.IsNullOrEmpty(cssClass) && cssClass != null)
            {
                htmlParagraph.CssClass = cssClass;
            }
            
            // Creates the paragraph
            htmlReportCreator.HtmlReporter.Add(htmlParagraph);
            
            bool GetDataEvaluation(out IElement? element)
            {
                var viewNode = reportNodeOrigin.getOrDefault<IElement>(_Reports._ReportParagraph.viewNode);
                if (viewNode == null)
                {
                    htmlReportCreator.HtmlReporter.Add(new HtmlParagraph("No Viewnode found"));
                    element = null;
                    return true;
                }

                var dataViewEvaluation = htmlReportCreator.GetDataViewEvaluation();
                element = dataViewEvaluation.GetElementsForViewNode(viewNode).OfType<IElement>().FirstOrDefault();
                if (element == null)
                {
                    htmlReportCreator.HtmlReporter.Add(new HtmlParagraph("No Element found"));
                    return true;
                }

                return false;
            }
        }
    }
}