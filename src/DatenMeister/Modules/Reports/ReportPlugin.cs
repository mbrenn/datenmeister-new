using DatenMeister.Integration;
using DatenMeister.Modules.Reports.Adoc;
using DatenMeister.Modules.Reports.Html;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.Modules.Reports
{
    [PluginLoading]
    public class ReportPlugin : IDatenMeisterPlugin
    {
        private readonly IScopeStorage _scopeStorage;

        public ReportPlugin(
            IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }
        
        public void Start(PluginLoadingPosition position)
        {
            _scopeStorage.Add(CreateAdocEvaluators());
            _scopeStorage.Add(CreateHtmlEvaluators());
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
            return evaluator;
        }

        /// <summary>
        /// Creates the default evaluator
        /// </summary>
        /// <returns>The default evaluators</returns>
        public static AdocReportEvaluators CreateAdocEvaluators()
        {
            var evaluator = new AdocReportEvaluators();
            evaluator.AddEvaluator(new AdocEvalReportTitle());
            evaluator.AddEvaluator(new AdocEvalReportParagraph());
            evaluator.AddEvaluator(new AdocEvalReportTable());
            return evaluator;
        }
    }
}