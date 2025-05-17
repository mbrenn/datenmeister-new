using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.DynamicFunctions;

namespace DatenMeister.Core.Runtime.Workspaces
{
    /// <summary>
    /// Defines the workspace logic is used to organize the workspaces
    /// </summary>
    public interface IWorkspaceLogic : IUriResolver
    {
        /// <summary>
        ///     Gets the scope storage, if set
        /// </summary>
        IScopeStorage? ScopeStorage { get; }

        /// <summary>
        ///     Gets the workspaces of the workspace logic
        /// </summary>
        IEnumerable<Workspace> Workspaces { get; }

        Workspace AddWorkspace(Workspace workspace);

        /// <summary>
        /// Removes a workspace containing the id
        /// </summary>
        /// <param name="id">Id of the workspace</param>
        void RemoveWorkspace(string id);

        /// <summary>
        /// Gets the workspace with specific id
        /// </summary>
        /// <param name="id">Id of the workspace</param>
        /// <returns>Found workspace or null, if not found</returns>
        Workspace? GetWorkspace(string id);

        /// <summary>
        /// Gets the dynamic function manager for a certain workspace.
        /// The workspace needs to be retrieved in which the types are hosted
        /// </summary>
        /// <param name="workspace">Workspace to be queried</param>
        /// <returns>The dynamic function manager</returns>
        DynamicFunctionManager GetDynamicFunctionManager(string workspace);

        /// <summary>
        /// Gets the datalayer of a certain extent
        /// </summary>
        /// <param name="extent"></param>
        /// <returns></returns>
        Workspace? GetWorkspaceOfExtent(IExtent extent);

        /// <summary>
        /// Gets the meta layer of a certain object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Workspace? GetWorkspaceOfObject(IObject value);

        /// <summary>
        /// Gets all extents for a specific datalayer
        /// </summary>
        /// <param name="dataLayer">Datalayer to be retrieved</param>
        /// <returns>enumeration of extents within the datalayer</returns>
        IEnumerable<IUriExtent> GetExtentsForWorkspace(Workspace dataLayer);

        /// <summary>
        /// Gets the default workspace
        /// </summary>
        /// <returns>The default workspace or null, if not found. </returns>
        Workspace? GetDefaultWorkspace();

        /// <summary>
        /// Adds an extent to the workspace
        /// </summary>
        /// <param name="workspace">Workspace to which the extent shall be added</param>
        /// <param name="newExtent">The extent to be added</param>
        void AddExtent(Workspace workspace, IUriExtent newExtent);

        /// <summary>
        /// Sends an event for a workspace change
        /// </summary>
        /// <param name="workspace">The workspace that has been changed</param>
        void SendEventForWorkspaceChange(Workspace workspace);
    }
}