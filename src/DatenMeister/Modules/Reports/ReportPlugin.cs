using DatenMeister.Integration;
using DatenMeister.Models.Reports;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Modules.Reports.Evaluators;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Runtime.Plugins;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.Reports
{
    [PluginLoading]
    public class ReportPlugin : IDatenMeisterPlugin
    {
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly FormsPlugin _formsPlugin;
        private readonly PackageMethods _packageMethods;
        private readonly IScopeStorage _scopeStorage;

        public ReportPlugin(
            LocalTypeSupport localTypeSupport, 
            FormsPlugin formsPlugin,
            PackageMethods packageMethods,
            IScopeStorage scopeStorage)
        {
            _localTypeSupport = localTypeSupport;
            _formsPlugin = formsPlugin;
            _packageMethods = packageMethods;
            _scopeStorage = scopeStorage;
        }
        
        public void Start(PluginLoadingPosition position)
        {
            _localTypeSupport.ImportTypes(
                ReportLogic.PackagePathTypesReport,
                _Reports.TheOne,
                IntegrateReports.Assign
            );
            
            _packageMethods.ImportByManifest(
                typeof(ReportPlugin),
                "DatenMeister.XmiFiles.Modules.Reports.xmi",
                "Forms",
                _formsPlugin.GetInternalFormExtent(),
                "Reports");
            
            _scopeStorage.Add(CreateEvaluators());
        }

        /// <summary>
        /// Creates the default evaluator
        /// </summary>
        /// <returns>The default evaluators</returns>
        private static HtmlReportEvaluators CreateEvaluators()
        {
            var evaluator = new HtmlReportEvaluators();
            evaluator.Evaluators.Add(new HtmlReportHeadline());
            evaluator.Evaluators.Add(new HtmlReportParagraph());
            evaluator.Evaluators.Add( new HtmlReportTable());
            return evaluator;
        }
    }
}