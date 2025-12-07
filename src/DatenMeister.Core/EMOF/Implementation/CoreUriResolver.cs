using BurnSystems.Logging;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.TypeIndexAssembly;

namespace DatenMeister.Core.EMOF.Implementation;

/// <summary>
/// Finds elements by a uri by walking through different workspaaces, extents.
/// The identification of the elements themselves is to be done by the different extents. 
/// </summary>
/// <param name="workspaceLogic">The workspace being used to perform the lookup</param>
public class CoreUriResolver(IWorkspaceLogic? workspaceLogic)
{
    /// <summary>
    /// Stores the logger is used to provide information in case resolving gives some information
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(CoreUriResolver));
    
    /// <summary>
    /// Performs the resolution 
    /// </summary>
    /// <param name="uri">Uri which is required to get resolved</param>
    /// <param name="resolveType">The type of how the unit shall get resolved</param>
    /// <param name="workspace">The workspace being queried</param>
    /// <param name="uriExtent">The extent to be queried</param>
    /// <returns>The found object or null in case nothing has been found</returns>
    public object? Resolve(string uri, ResolveType resolveType, IWorkspace? workspace = null, IUriExtent? uriExtent = null)
    {
        // Checks, if the item is in the current extent
        if ((resolveType.HasFlagFast(ResolveType.IncludeExtent)
             || resolveType.HasFlagFast(ResolveType.IncludeWorkspace))
            && uriExtent != null)
        {
            var result = uriExtent is MofUriExtent mofUriExtent
                ? mofUriExtent.Navigator.element(uri)
                : uriExtent.element(uri);

            if (result != null)
            {
                return result;
            }
        }
    
        // Checks, if the item is in the current workspace
        if (resolveType.HasFlagFast(ResolveType.IncludeWorkspace)
            && workspace != null)
        {
            var workspaceResult = workspace.Resolve(uri, resolveType, false);
            if (workspaceResult != null)
            {
                return workspaceResult;
            }
        }

        // Checks, if we need to look into the types workspace
        var alreadyVisited = new HashSet<IWorkspace>();
        if (resolveType.HasFlagFast(ResolveType.IncludeTypeWorkspace)
            && workspaceLogic != null)
        {
            var typesWorkspace = workspaceLogic.TryGetTypesWorkspace();
            if (typesWorkspace != null)
            {
                foreach (var result in
                         typesWorkspace.extent
                             .OfType<IUriExtent>()
                             .Select(extent => extent.GetUriResolver().Resolve(uri, ResolveType.IncludeWorkspace, false))
                             .Where(result => result != null))
                {
                    return result;
                }
                
                alreadyVisited.Add(typesWorkspace);
            }
        }
        // Checks, if we need to look into the metaworkspaces
        if (resolveType.HasFlagFast(ResolveType.IncludeNeighboringWorkspaces))
        {
            // First step, we parse the TypeIndexLogic which provides a quite fast access 
            if (workspaceLogic != null && workspace != null)
            {
                var typeIndexLogic = new TypeIndexLogic(workspaceLogic);
                var metaClasses =
                    typeIndexLogic.TypeIndexStore.Current?.FindWorkspace(workspace.id)?.NeighborWorkspaces;

                if (metaClasses != null)
                {
                    // Now look into the explicit metaworkspaces, if no specific constraint is given
                    foreach (var element in
                             metaClasses.SelectMany(x => workspaceLogic.GetWorkspace(x)?.extent ?? [])
                                 .OfType<IUriExtent>()
                                 .Select(metaExtent => metaExtent.element(uri))
                                 .Where(element => element != null))
                    {
                        return element;
                    }
                }
            }
            else
            {
                Logger.Info("No workspace logic found for querying neighboring workspaces");
            }
        }


        // Checks, if we need to look into the metaworkspaces
        if (resolveType.HasFlagFast(ResolveType.IncludeMetaWorkspaces))
        {
            // First step, we parse the TypeIndexLogic which provides a quite fast access 
            if (workspaceLogic != null && workspace != null)
            {
                var typeIndexLogic = new TypeIndexLogic(workspaceLogic);
                var metaClasses =
                    typeIndexLogic.TypeIndexStore.Current?.FindWorkspace(workspace.id)?.MetaclassWorkspaces;

                if (metaClasses != null)
                {
                    // Now look into the explicit metaworkspaces, if no specific constraint is given
                    foreach (var element in
                             metaClasses.SelectMany(x => workspaceLogic.GetWorkspace(x)?.extent ?? [])
                                 .OfType<IUriExtent>()
                                 .Select(metaExtent => metaExtent.element(uri))
                                 .Where(element => element != null))
                    {
                        return element;
                    }
                }
            }

            // If we have not found anything, we need to look into the meta extents being associated to that extent
            if (uriExtent is MofUriExtent mofExtent)
            {
                foreach (var metaExtent in mofExtent.MetaExtents)
                {
                    var result = metaExtent.GetUriResolver().Resolve(uri, ResolveType.IncludeExtent, false);
                    if (result != null) return result;
                }
            }
        }

        // If still not found, do a full search in every extent in every workspace
        if (resolveType.HasFlagFast(ResolveType.IncludeAll)
            && workspaceLogic != null)
        {
            foreach (var innerWorkspace in workspaceLogic.Workspaces)
            {
                if ((innerWorkspace as Workspace)?.IsDynamicWorkspace == true) continue;
                if (alreadyVisited.Contains(innerWorkspace))
                {
                    continue;
                }

                // Check, if there is an extent with the name
                var extent = innerWorkspace.FindExtent(uri);
                if (extent != null)
                {
                    return extent;
                }

                // If there is not an extent, then check, if there is an item
                foreach (var result in
                         innerWorkspace.extent
                             .OfType<IUriExtent>()
                             .Select(innerExtent => innerExtent.element(uri))
                             .Where(result => result != null))
                {
                    return result;
                }
            }
        }
        
        return null;
    }
}