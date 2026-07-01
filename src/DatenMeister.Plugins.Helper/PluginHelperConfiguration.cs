using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;

namespace DatenMeister.Plugins.Helper;

/// <summary>
/// Configures the main properties of the Plugin Helper which are common across the several plugin function
/// </summary>
public class PluginHelperConfiguration
{
    /// <summary>
    /// Defines the current plugin loading position as defined by the Start Function of
    /// IDatenMeisterPlugin realization.
    /// </summary>
    public PluginLoadingPosition Position { get; set; }
    
    /// <summary>
    /// Gets or Sets the current workspacelogic
    /// </summary>
    public required IWorkspaceLogic WorkspaceLogic { get; set; }
    
    /// <summary>
    /// Gets or sets the scopestorage
    /// </summary>
    public required IScopeStorage ScopeStorage { get; init; }
    
    /// <summary>
    /// Gets or sets the type of the plugin. This is also used to retrieve the namespace and assemblyname
    /// </summary>
    public required Type PluginType { get; init; }

    /// <summary>
    /// Gets the name of the assembly without .dll out of the PluginType
    /// </summary>
    public string AssemblyName
    {
        get
        {
            if (PluginType == null)
            {
                throw new InvalidOperationException("PluginType is not set");
            }
            
            return Path.GetFileNameWithoutExtension(
                PluginType.Assembly.ManifestModule.Name 
                ?? throw new InvalidOperationException("There was no AssemblyQualifedName. This should not happen."));
                
        }
    }
}