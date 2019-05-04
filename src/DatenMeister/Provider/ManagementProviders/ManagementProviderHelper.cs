using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Integration;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.ManagementProviders
{
    /// <summary>
    /// Defines the static helper
    /// </summary>
    public static class ManagementProviderHelper
    {
        /// <summary>
        /// Initializes the ExtentHelper and creates the extent for the workspaces
        /// </summary>
        /// <param name="workspaceLogic">The workspace logic to be used</param>
        public static void Initialize(IWorkspaceLogic workspaceLogic)
        {
            // Adds the extent containing the workpsaces
            workspaceLogic.GetManagementWorkspace().AddExtent(
                new MofUriExtent(new ExtentOfWorkspaces(workspaceLogic), WorkspaceNames.ExtentManagementExtentUri));
        }

        /// <summary>
        /// Gets the extent that contains the workspaces 
        /// </summary>
        /// <param name="scope">Scope for the DatenMeister</param>
        /// <returns>The found uri extent</returns>
        public static IUriExtent GetExtentsForWorkspaces(IDatenMeisterScope scope)
        {
            var workspaceLogic = scope.Resolve<IWorkspaceLogic>();
            return workspaceLogic.GetManagementWorkspace().FindExtent(WorkspaceNames.ExtentManagementExtentUri);
        }
    }
}