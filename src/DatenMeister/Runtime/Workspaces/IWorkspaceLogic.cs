using System.Collections.Generic;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;

namespace DatenMeister.Runtime.Workspaces
{
    public interface IWorkspaceLogic
    {
        Workspace AddWorkspace(Workspace workspace);

        void RemoveWorkspace(string id);

        Workspace GetWorkspace(string id);

        IEnumerable<Workspace> Workspaces { get; }

        void SetDefaultWorkspace(Workspace layer);

        /// <summary>
        /// Assigns a layer
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="dataLayer"></param>
        void AssignToWorkspace(IExtent extent, Workspace dataLayer);

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

        /// <summary>
        /// Gets the datalayer by name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Workspace GetById(string id);

        Workspace GetDefaultWorkspace();
    }
}