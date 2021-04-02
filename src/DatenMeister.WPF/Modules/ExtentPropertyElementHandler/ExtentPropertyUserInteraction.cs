using System;
using System.Collections.Generic;
using System.Windows;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Integration;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Extents.Configuration;
using DatenMeister.Runtime.ExtentStorage;
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

            var extent = _workspaceLogic.GetExtentByManagementModel(asElement);

            // Adds a button to read the extent loading configuration
            var extentManager = new ExtentManager(_workspaceLogic, _scopeStorage);
            var loadConfiguration = extentManager.GetLoadConfigurationFor(extent);
            if (loadConfiguration != null)
            {
                var interaction = new DefaultElementInteraction(
                    "Configure Load Configuration",
                    async (x, y) =>
                    {
                        await NavigatorForItems.NavigateToElementDetailView(
                            x.NavigationHost,
                            loadConfiguration,
                            title: $"Load Configuration for {extent.contextURI()}");
                    }
                );
                
                yield return interaction;
            }
            
            // Adds a new button for each of the possible configuration items that could be added to an extent 
            var extentSettings = _scopeStorage.Get<ExtentSettings>();
            foreach (var property in extentSettings.propertyDefinitions)
            {
                var newData = new DefaultElementInteraction(
                    $"Configure {property.title}",
                    async (x, y) =>
                    {
                        // Gets the extent to find the real extent by using the model.  
                        var foundElement = extent.getOrDefault<IElement>(property.name);
                        if (foundElement == null)
                        {
                            var factory = new MofFactory(extent);
                            foundElement = factory.create(property.metaClass);
                            extent.set(property.name, foundElement);
                            foundElement = extent.getOrDefault<IElement>(property.name);
                        }

                        if (foundElement == null)
                        {
                            throw new InvalidOperationException(
                                "Getting the data did not work for whatever reason");
                        }

                        var foundElementMetaClass = foundElement.getMetaClass();
                        var propertyMetaClass = property.metaClass;
                        if (foundElementMetaClass != null && propertyMetaClass != null &&
                            !foundElementMetaClass.@equals(property.metaClass))
                        {
                            var resolvedElementMetaClass =
                                _workspaceLogic.GetTypesWorkspace().ResolveElement(
                                    foundElementMetaClass,
                                    ResolveType.NoMetaWorkspaces,
                                    false) ?? foundElementMetaClass;

                            var resolvedPropertyMetaClass =
                                _workspaceLogic.GetTypesWorkspace().ResolveElement(
                                    propertyMetaClass,
                                    ResolveType.NoMetaWorkspaces,
                                    false) ?? propertyMetaClass;

                            if (MessageBox.Show(
                                $"The type of the configuration item is: {resolvedElementMetaClass}.\r\n\r\n" +
                                $"The type of the configuration item should be {resolvedPropertyMetaClass}.\r\n\r\n" +
                                $"Shall the type be converted?",
                                "Mismatch of type",
                                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                var asSetMetaclass = foundElement as IElementSetMetaClass;
                                asSetMetaclass?.SetMetaClass(propertyMetaClass);
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