using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.Provider.DotNet;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Plugins;

namespace DatenMeister.Extent.Manager
{
    public class SettingsProviderHelper : IDatenMeisterPlugin
    {
        private readonly IScopeStorage _scopeStorage;

        private readonly IWorkspaceLogic _workspaceLogic;

        public SettingsProviderHelper(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }

        public void Start(PluginLoadingPosition position)
        {
            var typesWorkspace = _workspaceLogic.GetTypesWorkspace();
            var dotNetProvider = new ManagementSettingsProvider(new WorkspaceDotNetTypeLookup(typesWorkspace));
            var settingsExtent =
                new MofUriExtent(dotNetProvider, WorkspaceNames.UriExtentSettings, _scopeStorage);

            // Adds the extent containing the settings
            _workspaceLogic.GetManagementWorkspace().AddExtent(settingsExtent);

            var settings = _scopeStorage.Get<ExtentSettings>();
            var settingsObject = new DotNetProviderObject(dotNetProvider, settings);
            settingsExtent.elements().add(settingsObject);
        }
    }
}