using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    /// Defines the workspace logic is used to organize the workspaces
    /// </summary>
    public interface IWorkspaceLogic
    {
        void AddWorkspace(Workspace workspace);

        void RemoveWorkspace(string id);

        Workspace GetWorkspace(string id);

        IEnumerable<Workspace> Workspaces { get; }

        /// <summary>
        /// Gets the datalayer of a certain extent
        /// </summary>
        /// <param name="extent"></param>
        /// <returns></returns>
        Workspace GetWorkspaceOfExtent(IExtent extent);

        /// <summary>
        /// Gets the meta layer of a certain object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Workspace GetWorkspaceOfObject(IObject value);

        /// <summary>
        /// Gets all extents for a specific datalayer
        /// </summary>
        /// <param name="dataLayer">Datalayer to be retrieved</param>
        /// <returns>enumeration of extents within the datalayer</returns>
        IEnumerable<IUriExtent> GetExtentsForWorkspace(Workspace dataLayer);

        Workspace GetDefaultWorkspace();
    }
}