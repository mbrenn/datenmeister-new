using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.HtmlExporter.HtmlEngine;
using DatenMeister.Modules.TextTemplates;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Reports.Html
{
    public class HtmlReportParagraph : IHtmlReportEvaluator
    {
        public bool IsRelevant(IElement element)
        {
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_DatenMeister.TheOne.Reports.__ReportParagraph) == true;
        }

        public void Evaluate(HtmlReportCreator htmlReportCreator, IElement reportNodeOrigin)
        {
            var reportNode = htmlReportCreator.GetNodeWithEvaluatedProperties(reportNodeOrigin);

            var paragraph = reportNode.getOrDefault<string>(_DatenMeister._Reports._ReportParagraph.paragraph);
            
            // Evaluates the paragraph if required
            if (reportNode.isSet(_DatenMeister._Reports._ReportParagraph.evalParagraph))
            {
                // Dynamic evaluation
                if (htmlReportCreator.GetDataEvaluation(reportNodeOrigin, out var element) || element == null) return;

                var evalParagraph = reportNode.getOrDefault<string>(_DatenMeister._Reports._ReportParagraph.evalParagraph);
                paragraph = TextTemplateEngine.Parse(
                    evalParagraph,
                    new Dictionary<string, object> {["i"] = element});
            }
            
            var htmlParagraph = new HtmlParagraph(paragraph);

            // Gets the cssClass
            var cssClass = reportNode.getOrDefault<string>(_DatenMeister._Reports._ReportParagraph.cssClass);
            if (!string.IsNullOrEmpty(cssClass) && cssClass != null)
            {
                htmlParagraph.CssClass = cssClass;
            }

            // Creates the paragraph
            htmlReportCreator.HtmlReporter.Add(htmlParagraph);
        }
    }
}