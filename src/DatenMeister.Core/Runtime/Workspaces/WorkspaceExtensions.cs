﻿using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;

namespace DatenMeister.Core.Runtime.Workspaces;

public static class WorkspaceExtensions
{
    /// <summary>
    /// Resolves the element of the workspace, if an object shadow extent is given.
    /// Otherwise, just returns the element itself
    /// </summary>
    public static IElement? ResolveElement(this Workspace workspace, IElement element)
    {
        if (element is MofObjectShadow shadowObject)
        {
            return workspace.Resolve(shadowObject.Uri, ResolveType.NoMetaWorkspaces) as IElement;
        }

        return element;
    }

    public static object? FindObjectOrCollection(this IWorkspaceLogic workspaceLogic, string workspaceId,
        string uri)
    {
        var workspace = string.IsNullOrEmpty(workspaceId)
            ? workspaceLogic.GetDefaultWorkspace()
            : workspaceLogic.GetWorkspace(workspaceId);

        return workspace?.Resolve(uri, ResolveType.NoMetaWorkspaces, true, workspaceId);
    }

    public static IObject? FindObject(this IWorkspaceLogic workspaceLogic, string workspaceId, string uri)
    {
        return FindObjectOrCollection(workspaceLogic, workspaceId, uri) as IObject;
    }

    public static IObject? FindObject(this Workspace workspace, string uri)
    {
        return FindObject(
            workspace.extent.Select(x => x as IUriExtent)
                .Where(x => x != null)!,
            uri);
    }

    public static IObject? FindObject(this IEnumerable<IUriExtent> extents, string uri)
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
        
    public static IElement? FindElement(this IWorkspaceLogic workspaceLogic, string workspace, string uri)
    {
        return workspaceLogic.FindObject(workspace, uri) as IElement;
    }
        
    /// <summary>
    /// Finds a certain object 
    /// </summary>
    /// <param name="workspaceLogic">Workspace logic to be queried</param>
    /// <param name="workspace">Workspace in which it shall be looked into</param>
    /// <param name="extentUri">Extent to be queried</param>
    /// <param name="itemId">Id of the item</param>
    /// <returns>The Found object or null if not found</returns>
    public static IElement? FindElement(
        this IWorkspaceLogic workspaceLogic, 
        string workspace, 
        string extentUri,
        string itemId)
    {
        var extent = workspaceLogic.FindExtent(workspace, extentUri);
        return extent?.element(itemId);
    }
        
    public static (IWorkspace workspace, IUriExtent extent, IElement element) FindItem(
        this IWorkspaceLogic collection,
        WorkspaceExtentAndItemReference model)
    {
        var (foundWorkspace, foundExtent) = FindWorkspaceAndExtent(collection, model);
        if (foundWorkspace == null || foundExtent == null)
        {
            throw new InvalidOperationException($"Element {model.item} has not been found");
        }

        var foundItem = foundExtent.element(model.item);
        if (foundItem == null)
        {
            throw new InvalidOperationException();
        }

        return (foundWorkspace, foundExtent, foundItem);
    }
        

    /// <summary>
    /// Finds the extent with the given uri in one of the workspaces in the database
    /// </summary>
    /// <param name="collection">Collection to be evaluated</param>
    /// <param name="uri">Uri, which needs to be retrieved</param>
    /// <returns>Found extent or null if not found</returns>
    public static IElement? FindElement(
        this IWorkspaceLogic collection,
        string uri)
    {
        return collection.Workspaces
            .SelectMany(x => x.extent)
            .Select(x => (x as IUriExtent)?.element(uri))
            .FirstOrDefault(x => x != null);
    }

    public static Workspace? FindWorkspace(this IEnumerable<Workspace> workspaces, IUriExtent extent)
    {
        return workspaces.FirstOrDefault(x => x.extent.Contains(extent));
    }

    /// <summary>
    /// Finds an extent by the given uri
    /// </summary>
    /// <param name="workspace">The workspace being queried</param>
    /// <param name="extentUri">The uri of the extent that is looked for</param>
    /// <returns>The found extent or null, if not found</returns>
    public static IUriExtent? FindExtent(this IWorkspace workspace, string extentUri)
    {
        return (IUriExtent?)workspace.extent.FirstOrDefault(
            x => (x as IUriExtent)?.contextURI() == extentUri
                 || (x as MofUriExtent)?.AlternativeUris.Contains(extentUri) == true);
    }

    /// <summary>
    /// Adds an extent to the workspace, but checks, if the given uri is already existing
    /// </summary>
    /// <param name="workspace">Workspace where extent will be added</param>
    /// <param name="workspaceLogic">Workspacelogic being used</param>
    /// <param name="extent">Extent being added</param>
    /// <returns>true, if addition was succesfsul</returns>
    public static bool AddExtentNoDuplicate(this Workspace workspace, IWorkspaceLogic workspaceLogic,
        IUriExtent extent)
    {
        var contextUri = extent.contextURI();

        var found = workspace.extent.FirstOrDefault(
            x => x is IUriExtent uriExtent
                 && uriExtent.contextURI() == contextUri);

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
    public static bool RemoveExtent(this Workspace workspace, string uri) => workspace.RemoveExtent(uri);

    /// <summary>
    /// Returns the given workspace and extents out of the urls
    /// </summary>
    /// <param name="workspaceLogic">Workspace collection to be used</param>
    /// <param name="model">Model to be queried</param>
    /// <returns>The found workspace and the found extent</returns>
    public static (IWorkspace? foundWorkspace, IUriExtent? foundExtent) FindWorkspaceAndExtent(
        this IWorkspaceLogic workspaceLogic,
        WorkspaceExtentAndItemReference model)
    {
        return FindWorkspaceAndExtent(
            workspaceLogic,
            model.ws,
            model.extent);

    }

    /// <summary>
    /// Tries to find the workspace and extent by the name 
    /// </summary>
    /// <param name="workspaceLogic">The workspace logic to be used</param>
    /// <param name="ws">Workspace to be found</param>
    /// <param name="extent">Extent to be found</param>
    /// <returns>Tuple returning the workspace and extent</returns>
    public static (IWorkspace? workspace, IUriExtent? extent) FindWorkspaceAndExtent(
        this IWorkspaceLogic workspaceLogic, string? ws, string? extent)
    {
        if (ws == null) return (null, null);

        var foundWorkspace = workspaceLogic.Workspaces.FirstOrDefault(x => x.id == ws);
        if (foundWorkspace == null) return (null, null);
        if (extent == null) return (foundWorkspace, null);

        var foundExtent = foundWorkspace.extent.Cast<IUriExtent>().FirstOrDefault(x => x.contextURI() == extent);
        if (foundExtent == null)
        {
            return (foundWorkspace, null);
        }

        return (foundWorkspace, foundExtent);
    }

    public static IUriExtent GetExtentByManagementModel(this IWorkspaceLogic workspaceLogic, IElement modelElement)
    {
        if (modelElement.getMetaClass()?.equals(_Management.TheOne.__Extent) != true)
        {
            throw new InvalidOperationException(
                $"The given element is not of type {_Management.TheOne.__Extent}");
        }

        var workspaceId = modelElement.getOrDefault<string>(_Management._Extent.workspaceId);
        var uri = modelElement.getOrDefault<string>(_Management._Extent.uri);

        return workspaceLogic.FindExtent(workspaceId, uri) as IUriExtent
               ?? throw new InvalidOperationException("The extent is not found");
    }

    /// <summary>
    /// Returns the given workspace out of a collection to which the extent is associated
    /// </summary>
    /// <param name="workspaceCollection">The workspace collection to be queried</param>
    /// <param name="extent">The extent to which the workspace is required</param>
    /// <returns>Found workspace or null, if not found</returns>
    public static Workspace? FindWorkspace(
        this IWorkspaceLogic workspaceCollection,
        IExtent extent)
    {
        return workspaceCollection.Workspaces.FirstOrDefault(x => x.extent.Contains(extent));
    }

    public static Workspace GetOrCreateWorkspace(
        this IWorkspaceLogic workspaceLogic,
        string name)
    {
        var workspace = workspaceLogic.GetWorkspace(name);
        return workspace ?? workspaceLogic.AddWorkspace(new Workspace(name));
    }

    /// <summary>
    /// Finds the extent with the given uri in one of the workspaces in the database
    /// </summary>
    /// <param name="workspaceLogic">Collection to be evaluated</param>
    /// <param name="extentUri">Uri, which needs to be retrieved</param>
    /// <returns>Found extent or null if not found</returns>
    public static IUriExtent? FindExtent(
        this IWorkspaceLogic workspaceLogic,
        string extentUri)
    {
        return workspaceLogic.Workspaces
            .SelectMany(x => x.extent)
            .OfType<IUriExtent>()
            .Select(x => 
                x.GetUriResolver().Resolve(extentUri, ResolveType.NoMetaWorkspaces, false) as IUriExtent)
            .FirstOrDefault(x => x != null);
    }

    /// <summary>
    /// Finds the extent with the given uri in one of the workspaces in the database
    /// </summary>
    /// <param name="workspaceLogic">Collection to be evaluated</param>
    /// <param name="workspaceId">Id of the workspace to be added</param>
    /// <param name="extentUri">Uri, which needs to be retrieved</param>
    /// <returns>Found extent or null if not found</returns>
    public static IUriExtent? FindExtent(
        this IWorkspaceLogic workspaceLogic,
        string workspaceId,
        string extentUri)
    {
        return FindExtentAndCollection(workspaceLogic, workspaceId, extentUri).uriExtent;
    }

    /// <summary>
    /// Finds the extent with the given uri in one of the workspaces in the database.
    /// The query can reference to a collection (by filtering items or requesting descendents).
    /// The extent, containing the collection is independently returned to allow queries against
    /// extent-types or using other extent information.   
    /// </summary>
    /// <param name="workspaceLogic">Collection to be evaluated</param>
    /// <param name="workspaceId">Id of the workspace to be added</param>
    /// <param name="extentUri">Uri, which needs to be retrieved</param>
    /// <returns>Found extent or null if not found</returns>
    public static (IReflectiveCollection? collection, IUriExtent? uriExtent) 
        FindExtentAndCollection(
            this IWorkspaceLogic workspaceLogic,
            string workspaceId,
            string extentUri)
    {
        if (string.IsNullOrEmpty(extentUri))
        {
            throw new InvalidOperationException("ExtentUri is empty");
        }
        
        if (string.IsNullOrEmpty(workspaceId))
        {
            // If the workspace is empty return it itself
            var workspace = workspaceLogic.GetDefaultWorkspace();
            if (workspace == null) return (null, null);

            workspaceId = workspace.id;
        }

        var foundExtent = workspaceLogic.Workspaces
            .Where(x => x.id == workspaceId)
            .SelectMany(x => x.extent)
            .OfType<IUriExtent>()
            .Select(x =>
                x.GetUriResolver().Resolve(extentUri, ResolveType.NoMetaWorkspaces | ResolveType.NoWorkspace, false))
            .FirstOrDefault(x => x != null);

        switch (foundExtent)
        {
            case IUriExtent asExtent:
                return (asExtent.elements(), asExtent);
            case IReflectiveCollection collection:
                return (collection, collection.GetUriExtentOf() as IUriExtent);
            case IElement element:
                return (new TemporaryReflectiveCollection(new[] { element }), element.GetUriExtentOf());
        }

        return (null, null);
    }

    /// <summary>
    /// Finds the extent and workspace and returns it as a tuple
    /// </summary>
    /// <param name="collection">Collection to be used</param>
    /// <param name="workspaceId">Id of the workspace being queried</param>
    /// <param name="extentUri">Uri of the extent being used</param>
    /// <param name="workspace">The found workspace</param>
    /// <param name="extent">The found extent</param>
    /// <returns>The tuple containing workspace and extent</returns>
    public static (IWorkspace? workspace, IExtent? extent) FindExtentAndWorkspace(
        this IWorkspaceLogic collection,
        string workspaceId,
        string extentUri)
    {
        var workspace = collection.Workspaces
            .FirstOrDefault(x => x.id == workspaceId);
        var extent = workspace?.extent
            .FirstOrDefault(x => (x as IUriExtent)?.contextURI() == extentUri);

        return (workspace, extent);
    }

    /// <summary>
    /// Gets all elements of all extents within the workspace
    /// </summary>
    /// <param name="workspace">Workspace to be evaluated.
    /// All extents are parsed within the workspace</param>
    /// <returns>Enumeration of all objects</returns>
    public static IEnumerable<IObject> GetAllElements(this IWorkspace workspace)
    {
        foreach (var extent in workspace.extent)
        {
            foreach (var element in extent.elements().OfType<IObject>())
            {
                yield return element;
            }
        }
    }

    public static Workspace? TryGetManagementWorkspace(this IWorkspaceLogic logic) =>
        logic.GetWorkspace(WorkspaceNames.WorkspaceManagement);

    public static Workspace GetManagementWorkspace(this IWorkspaceLogic logic) =>
        logic.GetWorkspace(WorkspaceNames.WorkspaceManagement)
        ?? throw new InvalidOperationException("Management is not found");

    public static Workspace GetDataWorkspace(this IWorkspaceLogic logic) =>
        logic.GetWorkspace(WorkspaceNames.WorkspaceData)
        ?? throw new InvalidOperationException("Data is not found");

    public static Workspace GetTypesWorkspace(this IWorkspaceLogic logic) =>
        logic.GetWorkspace(WorkspaceNames.WorkspaceTypes)
        ?? throw new InvalidOperationException("Types is not found");

    public static Workspace? TryGetTypesWorkspace(this IWorkspaceLogic logic) =>
        logic.GetWorkspace(WorkspaceNames.WorkspaceTypes);

    public static Workspace GetViewsWorkspace(this IWorkspaceLogic logic) =>
        logic.GetWorkspace(WorkspaceNames.WorkspaceViews)
        ?? throw new InvalidOperationException("Views is not found");

    public static IUriExtent GetUserFormsExtent(this IWorkspaceLogic logic)
    {
        var mgmt = GetManagementWorkspace(logic);
        return mgmt.FindExtent(WorkspaceNames.UriExtentUserForm)
               ?? throw new InvalidOperationException("User Forms not Found");
    }

    public static IUriExtent GetInternalFormsExtent(this IWorkspaceLogic logic)
    {
        var mgmt = GetManagementWorkspace(logic);
        return mgmt.FindExtent(WorkspaceNames.UriExtentInternalForm)
               ?? throw new InvalidOperationException("Internal Forms not Found");
    }

    public static Workspace GetUmlWorkspace(this IWorkspaceLogic logic) =>
        logic.GetWorkspace(WorkspaceNames.WorkspaceUml) ??
        throw new InvalidOperationException("Uml Workspace not found");

    public static Workspace GetMofWorkspace(this IWorkspaceLogic logic) =>
        logic.GetWorkspace(WorkspaceNames.WorkspaceMof) ??
        throw new InvalidOperationException("Mof Workspace not found");
        
        
        
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