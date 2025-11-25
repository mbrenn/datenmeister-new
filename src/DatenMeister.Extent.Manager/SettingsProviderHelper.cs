using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Provider.DotNet;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Manager;

public class SettingsProviderHelper(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) : IDatenMeisterPlugin
{
    public Task Start(PluginLoadingPosition position)
    {
        var typesWorkspace = workspaceLogic.GetTypesWorkspace();
        var dotNetProvider = new ManagementSettingsProvider(new WorkspaceDotNetTypeLookup(typesWorkspace));
        var settingsExtent =
            new MofUriExtent(dotNetProvider, WorkspaceNames.UriExtentSettings, scopeStorage);

        // Adds the extent containing the settings
        workspaceLogic.GetManagementWorkspace().AddExtent(settingsExtent);

        var settings = scopeStorage.Get<ExtentSettings>();
        var settingsObject = new DotNetProviderObject(dotNetProvider, settings);
        settingsExtent.elements().add(settingsObject);

        return Task.CompletedTask;
    }
}