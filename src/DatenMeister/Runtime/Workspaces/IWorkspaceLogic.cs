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

        void SetDefaultDatalayer(Workspace layer);

        /// <summary>
        /// Sets the relationship between two layers
        /// </summary>
        /// <param name="dataLayer">Layer to be allocated to another layer</param>
        /// <param name="metaDataLayer">Layer being allocated</param>
        void SetRelationShip(Workspace dataLayer, Workspace metaDataLayer);

        /// <summary>
        /// Assigns a layer
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="dataLayer"></param>
        void AssignToDataLayer(IExtent extent, Workspace dataLayer);

        /// <summary>
        /// Gets the datalayer of a certain extent
        /// </summary>
        /// <param name="extent"></param>
        /// <returns></returns>
        Workspace GetDataLayerOfExtent(IExtent extent);

        /// <summary>
        /// Gets the meta layer of a certain object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Workspace GetDataLayerOfObject(IObject value);

        /// <summary>
        /// Gets the meta layer for the given data layer
        /// </summary>
        /// <param name="data">Datalayer to be queried</param>
        /// <returns>The corresponding datalayer</returns>
        Workspace GetMetaLayerFor(Workspace data);

        /// <summary>
        /// Gets all extents for a specific datalayer
        /// </summary>
        /// <param name="dataLayer">Datalayer to be retrieved</param>
        /// <returns>enumeration of extents within the datalayer</returns>
        IEnumerable<IUriExtent> GetExtentsForDatalayer(Workspace dataLayer);

        /// <summary>
        /// Gets the datalayer by name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Workspace GetById(string id);
    }
}