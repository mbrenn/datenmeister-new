using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister
{
    public static class WorkspaceExtension
    {
        public static IObject FindElementByUri<T>(this Workspace<T> workspace, string uri) where T : IExtent
        {
            foreach (var extent in workspace.extent)
            {
                var extentAsUriExtent = extent as IUriExtent;
                var result = extentAsUriExtent?.element(uri);
                if (result != null)
                {
                    // found it
                    return result;
                }
            }

            // Not found
            return null;
        }

        public static Workspace<T> FindWorkspace<T>(this IEnumerable<Workspace<T>> workspaces, T extent)
            where T : IExtent
        {
            return workspaces.FirstOrDefault(x => x.extent.Contains(extent));
        }
    }
}