using DatenMeister.Models.Reports;
using DatenMeister.Modules.Forms.FormFinder;
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

        public ReportPlugin(
            LocalTypeSupport localTypeSupport, 
            FormsPlugin formsPlugin,
            PackageMethods packageMethods)
        {
            _localTypeSupport = localTypeSupport;
            _formsPlugin = formsPlugin;
            _packageMethods = packageMethods;
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
        }
    }
}