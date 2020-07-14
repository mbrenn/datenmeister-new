using DatenMeister.Models.AttachedExtent;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.Modules.AttachedExtent
{
    [PluginLoading(PluginLoadingPosition.AfterInitialization)]
    public class AttachedExtentPlugin : IDatenMeisterPlugin
    {
        private LocalTypeSupport _localTypeSupport;

        public AttachedExtentPlugin(LocalTypeSupport localTypeSupport)
        {
            _localTypeSupport = localTypeSupport;
        }

        public void Start(PluginLoadingPosition position)
        {
            if ((position & PluginLoadingPosition.AfterInitialization) != 0)
            {
                _localTypeSupport.AddInternalTypes("DatenMeister::AttachedExtent", AttachedExtentTypes.GetTypes());
            }
        }
    }
}