using System;
using System.Linq;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Workspaces
{
    public static class Extension
    {
        /// <summary>
        /// Adds an extent to the workspace, but checks, if the given uri is already existing
        /// </summary>
        /// <typeparam name="T">Type of the extents in the workspace</typeparam>
        /// <typeparam name="Q">Type of the extent being added. Needs to be of type IUriExtent</typeparam>
        /// <param name="workspace">Workspace where extent will be added</param>
        /// <param name="extent">Extent being added</param>
        /// <returns>true, if addition was succesfsul</returns>
        public static bool AddExtentNoDuplicate<T, Q>(this Workspace<T> workspace, Q extent) where T : IExtent where Q : T, IUriExtent
        {
            var contextUri = extent.contextURI();
            var found = workspace.extent.FirstOrDefault(x =>
            {
                var uriExtent = x as IUriExtent;
                return uriExtent != null && uriExtent.contextURI() == contextUri;
            });

            if (found == null)
            {
                workspace.AddExtent(extent);
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