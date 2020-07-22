using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.AttachedExtent;
using DatenMeister.Runtime;
using DatenMeister.WPF.Modules.UserInteractions;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.AttachedExtent
{
    public class AttachedExtentUserInteraction : IElementInteractionsHandler
    {
        private readonly AttachedExtentHandler _attachedExtentHandler;

        public AttachedExtentUserInteraction(AttachedExtentHandler attachedExtentHandler)
        {
            _attachedExtentHandler = attachedExtentHandler;
        }

        public IEnumerable<IElementInteraction> GetInteractions(IObject element)
        {
            if (!(element is IElement asElement))
            {
                yield break;
            }
            
            var uriExtent = element.GetUriExtentOf();
            if (uriExtent == null)
            {
                yield break;
            }
            
            var attachedExtents = _attachedExtentHandler.FindAttachedExtents(uriExtent);
            foreach (var attachedExtent in attachedExtents)
            {
                var configuration = _attachedExtentHandler.GetConfiguration(attachedExtent);
                if (configuration == null) continue;

                var interaction = new UserInteractions.DefaultElementInteraction(
                    $"Attach Item: {configuration.name ?? "Attached Item"}",
                    async (guest, o) =>
                    {
                        var attachedItem =
                            _attachedExtentHandler.GetOrCreateAttachedItem(asElement, attachedExtent);
                        await NavigatorForItems.NavigateToElementDetailView(guest.NavigationHost, attachedItem);
                    });
                yield return interaction;
            }
        }
    }
}