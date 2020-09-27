using DatenMeister.Integration;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.Reports.Adoc
{
    public class AdocReportCreator : ReportCreator
    {
        public AdocReportCreator(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
            : base(workspaceLogic, scopeStorage)
        {
        }
    }
}