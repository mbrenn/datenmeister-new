using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Core.Helper
{
    /// <summary>
    /// Contains several static helper methods to gain access 
    /// </summary>
    public static class WorkspaceHelper
    {
        /// <summary>
        /// Finds a certain object 
        /// </summary>
        /// <param name="workspaceLogic">Workspace logic to be queried</param>
        /// <param name="workspace">Workspace in which it shall be looked into</param>
        /// <param name="extentUri">Extent to be queried</param>
        /// <param name="itemId">Id of the item</param>
        /// <returns>The Found object or null if not found</returns>
        public static IElement? FindObject(
            this IWorkspaceLogic workspaceLogic, 
            string workspace, 
            string extentUri,
            string itemId)
        {
            var extent = workspaceLogic.FindExtent(workspace, extentUri) as IUriExtent;
            if (extent == null)
            {
                return null;
            }

            return extent.element(itemId);
        }
        
        /// <summary>
        /// Gets all root elements of all the extents within the workspace
        /// </summary>
        /// <param name="workspace">Workspace to be handled</param>
        /// <returns>Enumeration of all elements</returns>
        public static IReflectiveCollection GetRootElements(Workspace workspace)
        {
            return new TemporaryReflectiveCollection(workspace.extent.SelectMany(
                extent => extent.elements()).OfType<IElement>())
            {
                IsReadOnly = true
            };
        }

        public static IReflectiveCollection GetAllDescendents(this Workspace workspace, bool onlyComposite = true)
        {
            if (onlyComposite)
            {
                return GetRootElements(workspace).GetAllCompositesIncludingThemselves();
            }

            return GetRootElements(workspace).GetAllDescendantsIncludingThemselves();
        }

        public static IReflectiveCollection GetAllDescendentsOfType(this Workspace workspace, IElement metaClass, bool onlyComposite = true)
        {
            if (onlyComposite)
            {
                return GetRootElements(workspace).GetAllCompositesIncludingThemselves()
                    .WhenMetaClassIs(metaClass);
            }

            return GetRootElements(workspace).GetAllDescendantsIncludingThemselves()
                .WhenMetaClassIs(metaClass);
        }
    }
}