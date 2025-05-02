using System.Reflection;
using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Plugins;
using DatenMeister.Types;

namespace DatenMeister.Zip;


[PluginLoading(PluginLoadingPosition.AfterLoadingOfExtents)]
public class Plugin : IDatenMeisterPlugin
{
    private readonly IWorkspaceLogic _workspaceLogic;
    private readonly IScopeStorage _scopeStorage;

    public Plugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        _workspaceLogic = workspaceLogic;
        _scopeStorage = scopeStorage;
    }
    
    public Task Start(PluginLoadingPosition position)
    {
        LoadXmi();
        return Task.CompletedTask;
    }
    
    private void LoadXmi()
    {
        var localTypeSupport = new LocalTypeSupport(_workspaceLogic, _scopeStorage);
        var localTypeExtent = localTypeSupport.GetInternalTypeExtent();
        PackageMethods.ImportByStream(
            GetXmiStreamForTypes(), null, localTypeExtent, "DatenMeister.Zip");
    }

    public static Stream GetXmiStreamForTypes()
    {
        var stream = typeof(Plugin).GetTypeInfo()
            .Assembly.GetManifestResourceStream("DatenMeister.Zip.Xmi.DatenMeister.Zip.Types.xmi");
        return stream ?? throw new InvalidOperationException("Stream is not found");
    }
}