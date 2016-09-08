using System;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Workspaces
{
    public static class Extension
    {
        /// <summary>
        /// Finds an extent by the given uri
        /// </summary>
        /// <typeparam name="T">Type of the workspace</typeparam>
        /// <param name="workspace">The workspace being queried</param>
        /// <param name="uri">The uri of the extent that is looked for</param>
        /// <returns>The found extent or null, if not found</returns>
        public static IUriExtent FindExtent<T>(this Workspace<T> workspace, string uri) where T : IExtent
        {
            return (IUriExtent) workspace.extent.FirstOrDefault(x => (x as IUriExtent)?.contextURI() == uri);
        }

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

        /// <summary>
        /// Returns the given workspace and extents out of the urls
        /// </summary>
        /// <param name="workspaceCollection">Workspace collection to be used</param>
        /// <param name="model">Model to be queried</param>
        /// <param name="foundWorkspace">The found workspace</param>
        /// <param name="foundExtent">The found extent</param>
        public static void RetrieveWorkspaceAndExtent(
            this IWorkspaceCollection workspaceCollection,
            WorkspaceExtentAndItemReference model,
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

        /// <summary>
        /// Returns the given workspace out of a collection to which the extent is associated
        /// </summary>
        /// <param name="workspaceCollection">The workspace collection to be queried</param>
        /// <param name="extent">The extent to which the workspace is required</param>
        /// <returns>Found workspace or null, if not found</returns>
        public static Workspace<IExtent> FindWorkspace(
            this IWorkspaceCollection workspaceCollection,
            IExtent extent)
        {
            return workspaceCollection.Workspaces.FirstOrDefault(x => x.extent.Contains(extent));
        }



        /// <summary>
        /// Finds the extent with the given uri in one of the workspaces in the database
        /// </summary>
        /// <param name="collection">Collection to be evaluated</param>
        /// <param name="uri">Uri, which needs to be retrieved</param>
        /// <returns>Found extent or null if not found</returns>
        public static IExtent FindExtent(
            this IWorkspaceCollection collection,
            string uri)
        {
            return collection.Workspaces
                .SelectMany(x => x.extent)
                .FirstOrDefault(x => (x as IUriExtent)?.contextURI() == uri);
        }



        /// <summary>
        /// Finds the extent with the given uri in one of the workspaces in the database
        /// </summary>
        /// <param name="collection">Collection to be evaluated</param>
        /// <param name="uri">Uri, which needs to be retrieved</param>
        /// <returns>Found extent or null if not found</returns>
        public static IElement FindItem(
            this IWorkspaceCollection collection,
            string uri)
        {
            return collection.Workspaces
                .SelectMany(x => x.extent)
                .Select(x => (x as IUriExtent)?.element(uri))
                .FirstOrDefault(x => x != null);
        }

        public static void FindItem(
            this IWorkspaceCollection collection,
            WorkspaceExtentAndItemReference model,
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