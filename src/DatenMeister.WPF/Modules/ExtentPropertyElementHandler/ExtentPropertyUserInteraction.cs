﻿using System.Collections.Generic;
using System.Windows;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.ManagementProvider;
using DatenMeister.Models.Runtime;
using DatenMeister.Provider.ManagementProviders.Model;
using DatenMeister.Runtime;
using DatenMeister.WPF.Modules.UserInteractions;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.Modules.ExtentPropertyElementHandler
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
                    $"Create {property.title}",
                    () =>
                    {
                        var foundElement = element.getOrDefault<IElement>(property.name);
                        if (foundElement == null)
                        {
                            var factory = new MofFactory(element);
                            foundElement = factory.create(property.metaClass);
                        }

                    });

                yield return newData;
            }
        }
    }
}