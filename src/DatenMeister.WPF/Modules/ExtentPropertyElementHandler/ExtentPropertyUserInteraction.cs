using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Extents.Configuration;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Modules.UserInteractions;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ExtentPropertyElementHandler
{
    public class ExtentPropertyUserInteraction : IElementInteractionsHandler
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;

        public ExtentPropertyUserInteraction(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }

        public IEnumerable<IElementInteraction> GetInteractions(IObject element)
        {
            if (!(element is IElement asElement))
            {
                yield break;
            }

            if (asElement.getMetaClass()?.@equals(_DatenMeister.TheOne.Management.__Extent) != true)
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
                        // Gets the extent to find the real extent by using the model.  
                        var extent = _workspaceLogic.GetExtentByManagementModel(asElement);           
                        var foundElement = extent.getOrDefault<IElement>(property.name);
                        if (foundElement == null)
                        {
                            var factory = new MofFactory(extent);
                            foundElement = factory.create(property.metaClass);
                            extent.set(property.name, foundElement);
                            foundElement = extent.getOrDefault<IElement>(property.name);

                            if (foundElement == null)
                            {
                                throw new InvalidOperationException(
                                    "Getting the data did not work for whatever reason");
                            }
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