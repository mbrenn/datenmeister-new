using System.Data;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.DataViews
{
    public class DataViewLogic
    {
        /// <summary>
        /// Defines the path to the packages of the fast view filters
        /// </summary>
        public const string PackagePathTypesDataView = "DatenMeister::DataView";

        private readonly IWorkspaceLogic _workspaceLogic;

        public DataViewLogic(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }
        
    }
}