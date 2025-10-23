using DatenMeister.Core.Interfaces.MOF.Identifiers;

// ReSharper disable InconsistentNaming

namespace DatenMeister.Core.Interfaces.Workspace;

public interface IWorkspace : IUriResolver
{
    /// <summary>
    /// Clears the cache, so a new instance can be created
    /// </summary>
    void ClearCache();

    /// <summary>
    /// Gets a list of extents
    /// </summary>
    IEnumerable<IExtent> extent { get; }

    /// <summary>
    /// Gets the id of the workspace
    /// </summary>
    string id { get; }

    public string annotation { get; set; }

    /// <summary>
    /// Gets a list of workspaces which contain the types of the workspace
    /// </summary>
    List<IWorkspace> MetaWorkspaces { get; }

    void AddMetaWorkspace(IWorkspace innerWorkspace);
    
    void AddExtent(IExtent newExtent);

    bool RemoveExtent(IExtent extentToBeRemoved);
}