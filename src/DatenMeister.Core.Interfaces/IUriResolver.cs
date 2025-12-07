using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.Interfaces;

/// <summary>
/// Supports resolving uri to elements within the extent or even within other workspaces
/// </summary>
public interface IUriResolver
{
    /// <summary>
    /// Returns an element by resolving the uri
    /// </summary>
    /// <param name="uri">The uri to be used for resolving</param>
    /// <param name="resolveType">The type of resolve strategy</param>
    /// <param name="traceFailing">True, if a trace event shall be thrown, if
    /// the resolving did not succeed</param>
    /// <param name="workspace">Describes the workspace that can be used</param>
    /// <returns>The found element or null, if no element was found</returns>
    object? Resolve(string uri, ResolveType resolveType, bool traceFailing = true, string? workspace = null);

    /// <summary>
    /// Resolves a certain element by id
    /// </summary>
    /// <param name="id">Path to be queried</param>
    /// <returns>The found element for the id</returns>
    IElement? ResolveById(string id);
}

[Flags]
public enum ResolveType
{
    /// <summary>
    /// Default resolving process in which all extents in current workspace but also meta workspaces are resolved.
    /// If nothing was found, a full search will be started
    /// </summary>
    Default = IncludeExtent | IncludeWorkspace | IncludeMetaWorkspaces,

    /// <summary>
    /// Resolves only within the extent
    /// </summary> 
    IncludeExtent = 0x01,
    
    /// <summary>
    /// Resolves also within the workspace
    /// </summary>
    IncludeWorkspace = 0x02,
    
    /// <summary>
    /// Searches also within the workspace within the types
    /// </summary>
    IncludeTypeWorkspace = 0x04,
    
    /// <summary>
    /// Resolves also within the meta workspaces. The workspace itself is not included
    /// </summary>
    IncludeMetaWorkspaces = 0x08,
    
    /// <summary>
    /// We parse through everything! That is the most time-consuming resolving process. We should really avoid that. 
    /// </summary>
    IncludeAll = 0x10
}

public static class ResolveTypeExtensions
{
    public static bool HasFlagFast(this ResolveType value, ResolveType flag)
    {
        return (value & flag) != 0;
    }
}