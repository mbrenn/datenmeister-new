using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Plugins;

namespace DatenMeister.TemporaryExtent
{
    [PluginLoading(PluginLoadingPosition.AfterBootstrapping)]
    public class TemporaryExtentPlugin : IDatenMeisterPlugin
    {
        
        /// <summary>
        /// The Uri of the temporary extent
        /// </summary>
        public const string Uri = "dm:///_internal/temp";
        
        private readonly IWorkspaceLogic _workspaceLogic;

        public TemporaryExtentPlugin(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }
        
        /// <summary>
        /// Starts the plugin by creating the InMemoryProvider. The provider will be directly added to
        /// the workspace logic since it shall not be persisted. Upon restart of server, the data may be lost
        /// </summary>
        /// <param name="position">Position to be used</param>
        public void Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterBootstrapping:
                    var temporaryProvider = new InMemoryProvider();
                    var extent = new MofUriExtent(temporaryProvider, "dm:///_internal/temp", null);
                    _workspaceLogic.AddExtent(_workspaceLogic.GetDataWorkspace(), extent);
                    break;
            }
        }
    }
}