using DatenMeister.Integration;
using DatenMeister.Modules.Reports.Evaluators;
using DatenMeister.Modules.Reports.Html;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.Modules.Reports
{
    [PluginLoading]
    public class ReportPlugin : IDatenMeisterPlugin
    {
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly IScopeStorage _scopeStorage;

        public ReportPlugin(
            LocalTypeSupport localTypeSupport,
            IScopeStorage scopeStorage)
        {
            _localTypeSupport = localTypeSupport;
            _scopeStorage = scopeStorage;
        }
        
        public void Start(PluginLoadingPosition position)
        {
            _scopeStorage.Add(CreateEvaluators());
        }

        /// <summary>
        /// Creates the default evaluator
        /// </summary>
        /// <returns>The default evaluators</returns>
        public static HtmlReportEvaluators CreateEvaluators()
        {
            var evaluator = new HtmlReportEvaluators();
            evaluator.AddEvaluator(new HtmlReportHeadline());
            evaluator.AddEvaluator(new HtmlReportParagraph());
            evaluator.AddEvaluator(new HtmlReportTable());
            return evaluator;
        }
    }
}