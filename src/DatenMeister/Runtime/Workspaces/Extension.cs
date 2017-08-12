using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Workspaces
{
    public static class Extension
    {
        public static IObject FindElementByUri(this Workspace workspace, string uri)
        {
            return FindElementByUri(workspace.extent.Select(x => x as IUriExtent).Where(x => x != null), uri);
        }

        public static IObject FindElementByUri(this IEnumerable<IUriExtent> extents, string uri)
        {
            foreach (var extent in extents)
            {
                var extentAsUriExtent = extent;
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

        public static Workspace FindWorkspace(this IEnumerable<Workspace> workspaces, IUriExtent extent)
        {
            return workspaces.FirstOrDefault(x => x.extent.Contains(extent));
        }

        /// <summary>
        /// Gets the given class from the metalayer to the datalayer
        /// </summary>
        /// <typeparam name="TFilledType">Type that is queried</typeparam>
        /// <param name="logic">The logic being used fby this method</param>
        /// <param name="dataLayer">The datalayer that is requested</param>
        /// <returns>The instance of the type</returns>
        public static TFilledType GetFromMetaLayer<TFilledType>(
            this IWorkspaceLogic logic,
            Workspace dataLayer)
            where TFilledType : class, new()
        {
            var metaLayer = dataLayer.MetaWorkspace;
            return metaLayer?.Get<TFilledType>();
        }

        /// <summary>
        /// Gets the given class from the metalayer to the datalayer
        /// </summary>
        /// <typeparam name="TFilledType">Type that is queried</typeparam>
        /// <param name="logic">The logic being used fby this method</param>
        /// <param name="extent">The extent that is requested</param>
        /// <returns>The instance of the type</returns>
        public static TFilledType GetFromMetaLayer<TFilledType>(
            this IWorkspaceLogic logic,
            IExtent extent)
            where TFilledType : class, new()
        {
            var dataLayer = logic.GetWorkspaceOfExtent(extent);
            if (dataLayer == null)
            {
                return null;
            }

            return GetFromMetaLayer<TFilledType>(logic, dataLayer);
        }

        /// <summary>
        /// Finds an extent by the given uri
        /// </summary>
        /// <typeparam name="T">Type of the workspace</typeparam>
        /// <param name="workspace">The workspace being queried</param>
        /// <param name="uri">The uri of the extent that is looked for</param>
        /// <returns>The found extent or null, if not found</returns>
        public static IUriExtent FindExtent(this IWorkspace workspace, string uri)
        {
            return (IUriExtent) workspace.extent.FirstOrDefault(x => (x as IUriExtent)?.contextURI() == uri);
        }

        /// <summary>
        /// Adds an extent to the workspace, but checks, if the given uri is already existing
        /// </summary>
        /// <typeparam name="T">Type of the extents in the workspace</typeparam>
        /// <typeparam name="Q">Type of the extent being added. Needs to be of type IUriExtent</typeparam>
        /// <param name="workspace">Workspace where extent will be added</param>
        /// <param name="workspaceLogic">Workspacelogic being used</param>
        /// <param name="extent">Extent being added</param>
        /// <returns>true, if addition was succesfsul</returns>
        public static bool AddExtentNoDuplicate(this Workspace workspace, IWorkspaceLogic workspaceLogic, IUriExtent extent) 
        {
            var contextUri = extent.contextURI();
            var found = workspace.extent.FirstOrDefault(x =>
            {
                var uriExtent = x as IUriExtent;
                return uriExtent != null && uriExtent.contextURI() == contextUri;
            });

            if (found == null)
            {
                workspaceLogic.AddExtent(workspace, extent);
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
        public static bool RemoveExtent(this Workspace workspace, string uri)
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
            this IWorkspaceLogic workspaceCollection,
            WorkspaceExtentAndItemReference model,
            out Workspace foundWorkspace,
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
            this IWorkspaceLogic workspaceCollection,
            string ws,
            string extent,
            out Workspace foundWorkspace,
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
        public static Workspace FindWorkspace(
            this IWorkspaceLogic workspaceCollection,
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
            this IWorkspaceLogic collection,
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
            this IWorkspaceLogic collection,
            string uri)
        {
            return collection.Workspaces
                .SelectMany(x => x.extent)
                .Select(x => (x as IUriExtent)?.element(uri))
                .FirstOrDefault(x => x != null);
        }

        public static void FindItem(
            this IWorkspaceLogic collection,
            WorkspaceExtentAndItemReference model,
            out Workspace foundWorkspace,
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

        public static Workspace GetManagementWorkspace(this IWorkspaceLogic logic)
        {
            return logic.GetWorkspace(WorkspaceNames.NameManagement);
        }
        public static Workspace GetDataWorkspace(
            this IWorkspaceLogic logic)
        {
            return logic.GetWorkspace(WorkspaceNames.NameData);
        }

        public static Workspace GetTypesWorkspace(
            this IWorkspaceLogic logic)
        {
            return logic.GetWorkspace(WorkspaceNames.NameTypes);
        }

        /// <summary>
        /// Gets the data 
        /// </summary>
        /// <param name="scope">Scope of dependency container</param>
        /// <returns>The Uml instance being used</returns>
        public static _UML GetUmlData(this ILifetimeScope scope)
        {
            var workspaceLogic = scope.Resolve<IWorkspaceLogic>();
            return GetUmlData(workspaceLogic);
        }

        /// <summary>
        /// Gets the uml data from the workspace
        /// </summary>
        /// <param name="workspaceLogic">Workspace logic being used</param>
        /// <returns>The UML Data</returns>
        public static _UML GetUmlData(this IWorkspaceLogic workspaceLogic)
        {
            var uml = workspaceLogic.GetUmlWorkspace();
            return uml.Get<_UML>();
        }

        public static Workspace GetUmlWorkspace(
            this IWorkspaceLogic logic)
        {
            return logic.GetWorkspace(WorkspaceNames.NameUml);
        }

        public static Workspace GetMofWorkspace(
            this IWorkspaceLogic logic)
        {
            return logic.GetWorkspace(WorkspaceNames.NameMof);
        }
    }
}