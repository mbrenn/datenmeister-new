using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Forms;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Forms;

// ReSharper disable once UnusedType.Global
/// <summary>
/// Defines the default form extensions which are used to navigate through the
/// items, extents and also offers the simple creation and deletion of items. 
/// </summary>
public class TypesFormsPlugin : IDatenMeisterPlugin
{
    private readonly ExtentSettings _extentSettings;
    private readonly IWorkspaceLogic _workspaceLogic;
    private readonly IScopeStorage _scopeStorage;

    public TypesFormsPlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        _workspaceLogic = workspaceLogic;
        _scopeStorage = scopeStorage;
        _extentSettings = scopeStorage.Get<ExtentSettings>();
    }

    public Task Start(PluginLoadingPosition position)
    {
        switch (position)
        {
            case PluginLoadingPosition.AfterLoadingOfExtents:

                var formsPlugin = _scopeStorage.Get<FormsPluginState>();
                formsPlugin.FormModificationPlugins.Add(
                    new ExtentTypeFormModification(_workspaceLogic, _extentSettings));

                break;
        }

        return Task.CompletedTask;
    }
}