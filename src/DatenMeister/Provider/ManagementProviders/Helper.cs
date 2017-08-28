using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.HelpingExtents
{
    /// <summary>
    /// Defines the static helper
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Initializes the ExtentHelper and creates the extent for the workspaces
        /// </summary>
        /// <param name="workspaceLogic">The workspace logic to be used</param>
        public static void Initialize(IWorkspaceLogic workspaceLogic)
        {
            // Adds the extent containing the workpsaces
            workspaceLogic.GetManagementWorkspace().extent.Add(
                new MofUriExtent(new ExtentOfWorkspaces(workspaceLogic), Uris.WorkspaceUri));
        }
    }
}