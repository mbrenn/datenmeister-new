using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Integration;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.Provider.EnvironmentalVariableProvider
{
    [PluginLoading(PluginLoadingPosition.AfterInitialization |PluginLoadingPosition.AfterLoadingOfExtents)]
    // ReSharper disable once UnusedType.Global
    public class EnvironmentalVariablePlugin : IDatenMeisterPlugin
    {
        public const string DefaultExtentUri = "dm:///_internal/environmentalvariables/";
        
        private readonly IWorkspaceLogic _workspaceLogic;

        public EnvironmentalVariablePlugin(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            ScopeStorage = scopeStorage;
        }

        public IScopeStorage ScopeStorage { get; }

        public void Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterInitialization:
                {
                    var mapper = ScopeStorage.Get<ConfigurationToExtentStorageMapper>();
                    mapper.AddMapping(
                        _DatenMeister.TheOne.ExtentLoaderConfigs.__EnvironmentalVariableLoaderConfig,
                        manager => new EnvironmentalProvider());
                    break;
                }
                case PluginLoadingPosition.AfterLoadingOfExtents:
                    var mgmtProvider = _workspaceLogic.GetManagementWorkspace();
                    if (mgmtProvider.FindExtent(DefaultExtentUri) == null)
                    {
                        var extentLoader = new ExtentManager(_workspaceLogic, ScopeStorage);
                        var loaderConfig = InMemoryObject.CreateEmpty(
                            _DatenMeister.TheOne.ExtentLoaderConfigs.__EnvironmentalVariableLoaderConfig);
                        loaderConfig.set(
                            _DatenMeister._ExtentLoaderConfigs._EnvironmentalVariableLoaderConfig.extentUri,
                            DefaultExtentUri);
                        loaderConfig.set(
                            _DatenMeister._ExtentLoaderConfigs._EnvironmentalVariableLoaderConfig.workspaceId,
                            WorkspaceNames.WorkspaceManagement);
                        extentLoader.LoadExtent(loaderConfig);
                    }

                    break;
            }
        }
    }
}