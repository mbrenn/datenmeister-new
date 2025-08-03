using DatenMeister.Core;
using DatenMeister.Plugins;
using DatenMeister.Reports.Adoc;
using DatenMeister.Reports.Generic;
using DatenMeister.Reports.Html;

namespace DatenMeister.Reports;

[PluginLoading]
public class ReportPlugin(IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public Task Start(PluginLoadingPosition position)
    {
        scopeStorage.Add(CreateAdocEvaluators());
        scopeStorage.Add(CreateHtmlEvaluators());

        return Task.CompletedTask;
    }

    /// <summary>
    /// Creates the default evaluator
    /// </summary>
    /// <returns>The default evaluators</returns>
    public static HtmlReportEvaluators CreateHtmlEvaluators()
    {
        var evaluator = new HtmlReportEvaluators();
        evaluator.AddEvaluator(new HtmlEvalReportHeadline());
        evaluator.AddEvaluator(new HtmlReportParagraph());
        evaluator.AddEvaluator(new HtmlReportTable());
        evaluator.AddEvaluator(new GenericReportLoop<HtmlReportCreator>());
        return evaluator;
    }

    /// <summary>
    /// Creates the default evaluator
    /// </summary>
    /// <returns>The default evaluators</returns>
    public static AdocReportEvaluators CreateAdocEvaluators()
    {
        var evaluator = new AdocReportEvaluators();
        evaluator.AddEvaluator(new AdocEvalReportHeadline());
        evaluator.AddEvaluator(new AdocEvalReportParagraph());
        evaluator.AddEvaluator(new AdocEvalReportTable());
        evaluator.AddEvaluator(new GenericReportLoop<AdocReportCreator>());
        return evaluator;
    }
}