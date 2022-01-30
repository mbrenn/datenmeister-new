using DatenMeister.AttachedExtent;
using DatenMeister.Core;
using DatenMeister.Plugins;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.WPF.Modules.AttachedExtent
{
    public class AttachedExtentWpfPlugin : IDatenMeisterPlugin
    {
        private readonly AttachedExtentHandler _attachedExtentHandler;
        private readonly IScopeStorage _scopeStorage;

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