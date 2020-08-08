using DatenMeister.Integration;
using DatenMeister.Modules.AttachedExtent;
using DatenMeister.Runtime.Plugins;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.WPF.Modules.AttachedExtent
{
    public class AttachedExtentWpfPlugin : IDatenMeisterPlugin
    {
        private readonly IScopeStorage _scopeStorage;
        private readonly AttachedExtentHandler _attachedExtentHandler;

        public AttachedExtentWpfPlugin(IScopeStorage scopeStorage, AttachedExtentHandler attachedExtentHandler)
        {
            _scopeStorage = scopeStorage;
            _attachedExtentHandler = attachedExtentHandler;
        }

        public void Start(PluginLoadingPosition position)
        {
            _scopeStorage.Get<UserInteractionState>().ElementInteractionHandler.Add(
                new AttachedExtentUserInteraction(_attachedExtentHandler));
        }
    }
}