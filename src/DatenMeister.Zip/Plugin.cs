using System.Reflection;
using DatenMeister.Actions;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Plugins;
using DatenMeister.Types;
using DatenMeister.Zip.Logic;

namespace DatenMeister.Zip;


[PluginLoading()]
public class Plugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public Task Start(PluginLoadingPosition position)
    {
        // Loads the Xmi
        LoadXmi();
        
        // Add ActionHandler
        var actionState = scopeStorage.Get<ActionLogicState>();
        actionState.AddActionHandler(new ZipLogicActionHandler());
        
        return Task.CompletedTask;
    }
    
    private void LoadXmi()
    {
        var localTypeSupport = new LocalTypeSupport(workspaceLogic, scopeStorage);
        var localTypeExtent = localTypeSupport.GetInternalTypeExtent();
        PackageMethods.ImportByStream(
            scopeStorage, GetXmiStreamForTypes(), null, localTypeExtent, "DatenMeister.Zip");
    }

    private static Stream GetXmiStreamForTypes()
    {
        var stream = typeof(Plugin).GetTypeInfo()
            .Assembly.GetManifestResourceStream("DatenMeister.Zip.Xmi.DatenMeister.Zip.Types.xmi");
        return stream ?? throw new InvalidOperationException("Stream is not found");
    }
}