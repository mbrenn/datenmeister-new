using System.Reflection;
using DatenMeister.Actions;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Plugins;
using DatenMeister.Types;
using DatenMeister.Zip.Logic;

namespace DatenMeister.Zip;


/// <summary>
/// Defines the plugin for zip file handling within the DatenMeister environment.
/// </summary>
/// <param name="workspaceLogic">The workspace logic to be used.</param>
/// <param name="scopeStorage">The scope storage to be used.</param>
[PluginLoading()]
public class Plugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    /// <summary>
    /// Starts the plugin by loading the XMI and adding the action handler.
    /// </summary>
    /// <param name="position">The position at which the plugin is loaded.</param>
    /// <returns>A task representing the start process.</returns>
    public Task Start(PluginLoadingPosition position)
    {
        // Loads the Xmi
        LoadXmi();
        
        // Add ActionHandler
        var actionState = scopeStorage.Get<ActionLogicState>();
        actionState.AddActionHandler(new ZipLogicActionHandler());
        
        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Loads the XMI file containing the types for the zip handler.
    /// </summary>
    private void LoadXmi()
    {
        var localTypeSupport = new LocalTypeSupport(workspaceLogic, scopeStorage);
        var localTypeExtent = localTypeSupport.GetInternalTypeExtent();
        PackageMethods.ImportByStream(
            scopeStorage, GetXmiStreamForTypes(), null, localTypeExtent, "DatenMeister.Zip");
    }

    /// <summary>
    /// Gets the stream for the XMI file containing the types for the zip handler.
    /// </summary>
    /// <returns>The stream for the XMI file.</returns>
    private static Stream GetXmiStreamForTypes()
    {
        var stream = typeof(Plugin).GetTypeInfo()
            .Assembly.GetManifestResourceStream("DatenMeister.Zip.Xmi.DatenMeister.Zip.Types.xmi");
        return stream ?? throw new InvalidOperationException("Stream is not found");
    }
}