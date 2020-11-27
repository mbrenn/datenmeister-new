using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.TextTemplates;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Reports.Generic
{
    public abstract class GenericReportParagraph<T> :
        IGenericReportEvaluator<T> where T : GenericReportCreator
    {
        public bool IsRelevant(IElement element)
        {
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_DatenMeister.TheOne.Reports.__ReportParagraph) == true;
        }

        public void Evaluate(T htmlReportCreator, IElement reportNodeOrigin)
        {
            var reportNode = htmlReportCreator.GetNodeWithEvaluatedProperties(reportNodeOrigin, _DatenMeister._Reports._ReportParagraph.viewNode);

            var paragraph = reportNode.getOrDefault<string>(_DatenMeister._Reports._ReportParagraph.paragraph);

            // Evaluates the paragraph if required
            if (reportNode.isSet(_DatenMeister._Reports._ReportParagraph.evalParagraph))
            {
                // Dynamic evaluation
                if (htmlReportCreator.GetDataEvaluation(reportNodeOrigin, out var element, _DatenMeister._Reports._ReportParagraph.viewNode) || element == null) return;

                var evalParagraph = reportNode.getOrDefault<string>(_DatenMeister._Reports._ReportParagraph.evalParagraph);
                paragraph = TextTemplateEngine.Parse(
                    evalParagraph,
                    new Dictionary<string, object> { ["i"] = element });
            }

            var cssClass = reportNode.getOrDefault<string>(_DatenMeister._Reports._ReportParagraph.cssClass);

            WriteParagraph(htmlReportCreator, paragraph, cssClass);
        }

        public abstract void WriteParagraph(T report, string paragraph, string cssClass);
    }
}