using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Runtime
{
    /// <summary>
    /// Contains several static helper methods to gain access 
    /// </summary>
    public static class WorkspaceHelper
    {
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