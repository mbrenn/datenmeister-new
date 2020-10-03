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
            var reportNode = htmlReportCreator.GetNodeWithEvaluatedProperties(reportNodeOrigin);

            var paragraph = reportNode.getOrDefault<string>(_Reports._ReportParagraph.paragraph);
            var htmlParagraph = new HtmlParagraph(paragraph);

            // Gets the cssClass
            var cssClass = reportNode.getOrDefault<string>(_Reports._ReportParagraph.cssClass);
            if (!string.IsNullOrEmpty(cssClass) && cssClass != null)
            {
                htmlParagraph.CssClass = cssClass;
            }

            // Creates the paragraph
            htmlReportCreator.HtmlReporter.Add(htmlParagraph);
        }
    }
}