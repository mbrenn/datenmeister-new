﻿using System.Web;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Provider.ExtentManagement;

public class ExtentManagementUrlHelper
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

    public static string GetUrlOfExtent(Workspace workspace, IExtent? extent)
    {
        return
            $"{ManagementProviderPlugin.UriExtentWorkspaces}#{HttpUtility.UrlEncode(GetIdOfExtent(workspace, extent))}";
    }

    public static string GetUrlOfExtentsProperties(Workspace workspace, IExtent? extent)
    {
        return
            $"{ManagementProviderPlugin.UriExtentWorkspaces}#{HttpUtility.UrlEncode(GetIdOfExtentsProperties(workspace, extent))}";
    }
}