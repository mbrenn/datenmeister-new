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
                if (extentAsUriExtent != null)
                {
                    var result = extentAsUriExtent.element(uri);
                    if (result != null)
                    {
                        // found it
                        return result;
                    }
                }
            }

            // Not found
            return null;
        }
    }
}