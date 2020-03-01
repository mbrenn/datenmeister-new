using DatenMeister.Models.Reports;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.Modules.Reports
{
    [PluginLoading]
    public class ReportPlugin : IDatenMeisterPlugin
    {
        private readonly LocalTypeSupport _localTypeSupport;

        public ReportPlugin(LocalTypeSupport localTypeSupport)
        {
            _localTypeSupport = localTypeSupport;
        }
        
        public void Start(PluginLoadingPosition position)
        {
            _localTypeSupport.ImportTypes(
                ReportLogic.PackagePathTypesReport,
                _Reports.TheOne,
                IntegrateReports.Assign
            );
        }
    }
}