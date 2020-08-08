using System.Collections.Generic;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Runtime;
using DatenMeister.Provider.ManagementProviders.Model;
using DatenMeister.Runtime;
using DatenMeister.WPF.Modules.UserInteractions;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ExtentPropertyElementHandler
{
    public class ExtentPropertyUserInteraction : IElementInteractionsHandler
    {
        private readonly IScopeStorage _scopeStorage;

        public ExtentPropertyUserInteraction(IScopeStorage scopeStorage)
        {
            _scopeStorage = scopeStorage;
        }

        public IEnumerable<IElementInteraction> GetInteractions(IObject element)
        {
            if (!(element is IElement asElement))
            {
                yield break;
            }

            if (asElement.getMetaClass()?.@equals(_ManagementProvider.TheOne.__ExtentProperties) != true)
            {
                yield break;
            }
            
            var extentSettings = _scopeStorage.Get<ExtentSettings>();
            foreach (var property in extentSettings.propertyDefinitions)
            {
                var newData = new DefaultElementInteraction(
                    $"Configure {property.title}",
                    async (x,y) =>
                    {
                        var foundElement = element.getOrDefault<IElement>(property.name);
                        if (foundElement == null)
                        {
                            var factory = new MofFactory(element);
                            foundElement = factory.create(property.metaClass);
                            element.set(property.name, foundElement);
                        }

                        await NavigatorForItems.NavigateToElementDetailView(
                            x.NavigationHost,
                            foundElement,
                            title: $"Edit {property.title}");
                    });

                yield return newData;
            }
        }
    }
}