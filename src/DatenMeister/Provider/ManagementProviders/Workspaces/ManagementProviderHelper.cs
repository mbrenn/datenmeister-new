using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Integration;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.ManagementProviders.Workspaces
{
    /// <summary>
    /// Defines the static helper
    /// </summary>
    public static class ManagementProviderHelper
    {
        /// <summary>
        /// Initializes the ExtentHelper and creates the extent for the workspaces
        /// </summary>
        /// <param name="scope">The Dependency Injector</param>
        public static void Initialize(IDatenMeisterScope scope)
        {
            // Adds the extent containing the workpsaces
            scope.WorkspaceLogic.GetManagementWorkspace().AddExtent(
                new MofUriExtent(new ExtentOfWorkspaceProvider(scope), WorkspaceNames.UriExtentWorkspaces));
        }

        /// <summary>
        /// Gets the extent that contains the workspaces
        /// </summary>
        /// <param name="scope">Scope for the DatenMeister</param>
        /// <returns>The found uri extent</returns>
        public static IUriExtent GetExtentsForWorkspaces(IDatenMeisterScope scope)
        {
            var workspaceLogic = scope.Resolve<IWorkspaceLogic>();
            return workspaceLogic.GetManagementWorkspace().FindExtent(WorkspaceNames.UriExtentWorkspaces);
        }
    }
}