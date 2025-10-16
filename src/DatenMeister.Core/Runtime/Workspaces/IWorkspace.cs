using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Runtime.DynamicFunctions;

// ReSharper disable InconsistentNaming

namespace DatenMeister.Core.Runtime.Workspaces;

public interface IWorkspace
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
        
    /// <summary>
    /// Gets the dynamic function manager
    /// </summary>
    DynamicFunctionManager DynamicFunctionManager { get; }

    /// <summary>
    /// Gets a list of workspaces which contain the types of the workspace
    /// </summary>
    List<Workspace> MetaWorkspaces { get; }
}