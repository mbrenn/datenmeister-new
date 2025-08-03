using DatenMeister.AttachedExtent;
using DatenMeister.Core;
using DatenMeister.Plugins;
using DatenMeister.WPF.Modules.UserInteractions;

namespace DatenMeister.WPF.Modules.AttachedExtent;

public class AttachedExtentWpfPlugin(IScopeStorage scopeStorage, AttachedExtentHandler attachedExtentHandler)
    : IDatenMeisterPlugin
{
    public void Start(PluginLoadingPosition position)
    {
        scopeStorage.Get<UserInteractionState>().ElementInteractionHandler.Add(
            new AttachedExtentUserInteraction(attachedExtentHandler));
    }
}