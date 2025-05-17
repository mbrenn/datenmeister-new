using System.Web;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Provider.ExtentManagement
{
    public class ExtentManagementHelper
    {
        /// <summary>
        ///     Gets the management url of the workspace
        /// </summary>
        /// <param name="workspace">Workspace to be used</param>
        /// <returns>Returns the management url under which the workspace will be found</returns>
        public static string GetIdOfWorkspace(Workspace workspace)
        {
            return workspace.id;
        }
        /// <summary>
        ///     Gets the management url of the workspace
        /// </summary>
        /// <param name="workspace">Workspace to be used</param>
        /// <returns>Returns the management url under which the workspace will be found</returns>
        public static string GetIdOfWorkspace(string workspace)
        {
            return workspace;
        }

        /// <summary>
        ///     Gets the management url of the extent
        /// </summary>
        /// <param name="workspace">Workspace in which the extent is found</param>
        /// <param name="extent">Extent to be queried</param>
        /// <returns>The management url of the extent</returns>
        public static string GetIdOfExtent(Workspace workspace, IExtent? extent)
        {
            var extentUri =
                (extent as IUriExtent)?.contextURI() ??
                throw new InvalidOperationException("uriExtent and loadedExtentInformation is null");

            return workspace.id.Replace("_", "__") +
                   "_" +
                   extentUri.Replace("_", "__");
        }

        /// <summary>
        /// Gets the id of the extent by the name of the workspace and the uri of the extent
        /// </summary>
        /// <param name="workspaceName">Name of the workspace</param>
        /// <param name="extentUri">Uri of the extent</param>
        /// <returns>The Id of the extent</returns>
        public static string GetIdOfExtent(string workspaceName, string extentUri)
        {
            return workspaceName.Replace("_", "__") +
                   "_" +
                   extentUri.Replace("_", "__");
        }

        /// <summary>
        /// Gets the element in the management workspace reflecting the given workspace
        /// </summary>
        /// <param name="managementExtent">The management extent</param>
        /// <param name="workspace">Name of the workspace</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IElement GetWorkspaceElement(IUriExtent managementExtent, string workspace)
        {
            var workspaceId = GetIdOfWorkspace(workspace);
            return managementExtent.element(workspaceId)
                ?? throw new InvalidOperationException("Workspace Element was not found");
        }

        /// <summary>
        /// Gets the element in the management workspace reflecting the given extent
        /// </summary>
        /// <param name="managementExtent">The management extent</param>
        /// <param name="workspace">Name of the workspace</param>
        /// <param name="extentUri">Uri of the extent</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IElement GetExtentElement(IUriExtent managementExtent, string workspace, string extentUri)
        {
            var extentId = GetIdOfExtent(workspace, extentUri);
            return managementExtent.element(extentId)
                ?? throw new InvalidOperationException("Extent Element was not found");
        }

        /// <summary>
        ///     Gets the management url of the extent's properties
        /// </summary>
        /// <param name="workspace">Workspace in which the extent is found</param>
        /// <param name="extent">Extent to be queried</param>
        /// <returns>The management url of the extent's properties</returns>
        public static string GetIdOfExtentsProperties(Workspace workspace, IExtent? extent)
        {
            return $"{GetIdOfExtent(workspace, extent)}_Properties";
        }

        public static string GetUrlOfWorkspace(Workspace workspace)
        {
            return
                $"{ManagementProviderPlugin.UriExtentWorkspaces}#{HttpUtility.UrlEncode(GetIdOfWorkspace(workspace))}";
        }

        public static string GetUrlOfWorkspace(string workspaceName)
        {
            return
                $"{ManagementProviderPlugin.UriExtentWorkspaces}#{HttpUtility.UrlEncode(workspaceName)}";
        }

        public static string GetUrlOfExtent(Workspace workspace, IExtent? extent)
        {
            return
                $"{ManagementProviderPlugin.UriExtentWorkspaces}#{HttpUtility.UrlEncode(GetIdOfExtent(workspace, extent))}";
        }

        public static string GetUrlOfExtent(string workspaceName, string extentUri)
        {
            return
                $"{ManagementProviderPlugin.UriExtentWorkspaces}#{HttpUtility.UrlEncode(GetIdOfExtent(workspaceName, extentUri))}";
        }

        public static string GetUrlOfExtentsProperties(Workspace workspace, IExtent? extent)
        {
            return
                $"{ManagementProviderPlugin.UriExtentWorkspaces}#{HttpUtility.UrlEncode(GetIdOfExtentsProperties(workspace, extent))}";
        }

        /// <summary>
        ///     Gets the extent that contains the workspaces
        /// </summary>
        /// <param name="workspaceLogic">Logic for the workspace to be used</param>
        /// <returns>The found uri extent</returns>
        public static IUriExtent GetExtentForWorkspaces(IWorkspaceLogic workspaceLogic)
        {
            return workspaceLogic.GetManagementWorkspace().FindExtent(WorkspaceNames.UriExtentWorkspaces)
                   ?? throw new InvalidOperationException("Extent for uri extents not found");
        }
    }
}