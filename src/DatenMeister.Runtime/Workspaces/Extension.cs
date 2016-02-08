using System;
using System.Linq;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Workspaces
{
    public static class Extension
    {
        public static bool AddExtentNoDuplicate<T>(this Workspace<T> workspace, IUriExtent extent) where T : IExtent
        {
            var contextUri = extent.contextURI();
            var found = workspace.extent.FirstOrDefault(x =>
            {
                var uriExtent = x as IUriExtent;
                return uriExtent != null && uriExtent.contextURI() == contextUri;
            });

            if (found == null)
            {
                workspace.AddExtent((T) extent);
                return true;
            }

            throw new InvalidOperationException($"Extent with uri {contextUri} is already existing");
        }

        /// <summary>
        /// Removes the extent with the given uri out of the database
        /// </summary>
        /// <param name="workspace">The workspace to be modified</param>
        /// <param name="uri">Uri of the extent</param>
        /// <returns>true, if the object can be deleted</returns>
        public static bool RemoveExtent<T>(this Workspace<T> workspace, string uri) where T : IExtent
        {
            lock (workspace.SyncObject)
            {
                var found = workspace.extent.FirstOrDefault(x =>
                {
                    var uriExtent = x as IUriExtent;
                    return uriExtent != null && uriExtent.contextURI() == uri;
                });

                if (found != null)
                {
                    workspace.extent.Remove(found);
                    return true;
                }
            }

            return false;
        }

        public static void RetrieveWorkspaceAndExtent(
            this IWorkspaceCollection workspaceCollection,
            WorkspaceExtentAndItem model,
            out Workspace<IExtent> foundWorkspace,
            out IUriExtent foundExtent)
        {
            RetrieveWorkspaceAndExtent(
                workspaceCollection,
                model.ws,
                model.extent,
                out foundWorkspace,
                out foundExtent);
        }

        public static void RetrieveWorkspaceAndExtent(
            this IWorkspaceCollection workspaceCollection,
            string ws,
            string extent,
            out Workspace<IExtent> foundWorkspace,
            out IUriExtent foundExtent)
        {
            foundWorkspace = workspaceCollection.Workspaces.FirstOrDefault(x => x.id == ws);

            if (foundWorkspace == null)
            {
                throw new InvalidOperationException("Workspace_NotFound");
            }

            foundExtent = foundWorkspace.extent.Cast<IUriExtent>().FirstOrDefault(x => x.contextURI() == extent);
            if (foundExtent == null)
            {
                throw new InvalidOperationException("Extent_NotFound");
            }
        }

        public static void FindItem(
            this IWorkspaceCollection collection,
            WorkspaceExtentAndItem model,
            out Workspace<IExtent> foundWorkspace,
            out IUriExtent foundExtent,
            out IElement foundItem)
        {
            RetrieveWorkspaceAndExtent(collection, model, out foundWorkspace, out foundExtent);

            foundItem = foundExtent.element(model.item);
            if (foundItem == null)
            {
                throw new InvalidOperationException();
            }
        }
    }
}