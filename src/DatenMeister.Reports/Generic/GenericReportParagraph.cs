using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.TextTemplates;

namespace DatenMeister.Reports.Generic;

public abstract class GenericReportParagraph<T> :
    IGenericReportEvaluator<T> where T : GenericReportCreator
{
    public bool IsRelevant(IElement element)
    {
        var metaClass = element.getMetaClass();
        return metaClass?.equals(_Reports.TheOne.Elements.__ReportParagraph) == true;
    }

    public void Evaluate(ReportLogic reportLogic, T reportCreator, IElement reportNodeOrigin)
    {
        var reportNode = reportLogic.GetNodeWithEvaluatedProperties(reportNodeOrigin, _Reports._Elements._ReportParagraph.viewNode);

        var paragraph = reportNode.getOrDefault<string>(_Reports._Elements._ReportParagraph.paragraph);

        // Evaluates the paragraph if required
        if (reportNode.isSet(_Reports._Elements._ReportParagraph.evalParagraph))
        {
            // Dynamic evaluation
            if (!reportLogic.GetObjectViaDataEvaluation(
                    reportNodeOrigin, 
                    out var element, 
                    _Reports._Elements._ReportParagraph.viewNode) || element == null) return;

            var evalParagraph = reportNode.getOrDefault<string>(_Reports._Elements._ReportParagraph.evalParagraph);
            paragraph = TextTemplateEngine.Parse(
                evalParagraph,
                new Dictionary<string, object> { ["i"] = element });
        }

        var cssClass = 
            reportNode.getOrDefault<string>(_Reports._Elements._ReportParagraph.cssClass);

        WriteParagraph(reportCreator, paragraph, cssClass);
    }

    public abstract void WriteParagraph(T report, string paragraph, string cssClass);
}