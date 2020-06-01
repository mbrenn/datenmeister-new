﻿using System;
using System.Collections.Generic;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Plugin;
using DatenMeister.WPF.Forms;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.ListViews;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.TypeManager
{
    public class TypeManagerViewExtension : IViewExtensionFactory
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(TypeManagerViewExtension)); 
        public IEnumerable<ViewExtension> GetViewExtensions(
            ViewExtensionInfo viewExtensionInfo)
        {
            var navigationHost = viewExtensionInfo.NavigationHost ??
                                 throw new InvalidOperationException("NavigationHost == null");

            if (viewExtensionInfo.GetMainApplicationWindow() != null)
            {
                yield return new ApplicationMenuButtonDefinition(
                    "Goto User Types", async () => await NavigatorForItems.NavigateToItemsInExtent(
                        navigationHost,
                        WorkspaceNames.WorkspaceTypes,
                        WorkspaceNames.UriExtentUserTypes),
                    string.Empty,
                    NavigationCategories.DatenMeisterNavigation);
            }

            var itemExplorerControl = viewExtensionInfo.GetItemExplorerControlForExtentType(UmlPlugin.ExtentType);
            if (itemExplorerControl != null)
            {
                // Inject the buttons to create a new class or a new property (should be done per default, but at the moment per plugin)
                var extent = itemExplorerControl.Extent.GetExtentOf();
                if (extent != null)
                {
                    var classMetaClass = extent.FindInMeta<_UML>(x => x.StructuredClassifiers.__Class);

                    if (classMetaClass == null)
                    {
                        Logger.Warn("UML Classes not found in meta extent");
                    }
                    else
                    {
                        yield return new NewInstanceViewDefinition(classMetaClass);

                        yield return new ApplicationMenuButtonDefinition(
                            "Create new Class",
                            async () =>
                                await NavigatorForItems.NavigateToCreateNewItemInExtentOrPackage(
                                    navigationHost,
                                    itemExplorerControl.SelectedItem ?? itemExplorerControl.RootItem,
                                    classMetaClass),
                            string.Empty,
                            NavigationCategories.Type + "." + "Manager");
                    }
                }
            }

            var listControl = viewExtensionInfo.GetListViewForItemsTabForExtentType(UmlPlugin.ExtentType);
            if (listControl != null)
            {
                var extent = listControl.Extent;
                if (extent != null)
                {
                    var classMetaClass = extent.FindInMeta<_UML>(x => x.StructuredClassifiers.__Class);


                    if (classMetaClass == null)
                    {
                        Logger.Warn("UML Classes not found in meta extent");
                    }
                    else
                    {
                        yield return
                            new CollectionMenuButtonDefinition(
                                "Create new Class",
                                async (x) =>
                                    await NavigatorForItems.NavigateToNewItemForExtent(
                                        navigationHost,
                                        listControl.Extent,
                                        classMetaClass),
                                string.Empty,
                                NavigationCategories.Type);
                    }
                }
            }

            if (viewExtensionInfo is ViewExtensionItemPropertiesInformation propertiesInformation)
            {
                if (propertiesInformation.Value is IElement selectedPackage)
                {
                    var extent = selectedPackage.GetExtentOf();
                    if (extent != null)
                    {
                        var classMetaClass = extent.FindInMeta<_UML>(x => x.StructuredClassifiers.__Class);

                        var propertyName = propertiesInformation.Property;
                        if (selectedPackage.metaclass?.@equals(classMetaClass) == true
                            && propertyName == _UML._StructuredClassifiers._Class.ownedAttribute)
                        {
                            var propertyMetaClass = extent.FindInMeta<_UML>(x => x.Classification.__Property);
                            if (propertyMetaClass != null)
                            {
                                yield return new NewInstanceViewDefinition(propertyMetaClass);

                                yield return
                                    new CollectionMenuButtonDefinition(
                                        "Create new Property",
                                        async (x) =>
                                            await NavigatorForItems.NavigateToNewItemForPropertyCollection(
                                                navigationHost,
                                                selectedPackage,
                                                _UML._StructuredClassifiers._Class.ownedAttribute,
                                                propertyMetaClass),
                                        string.Empty,
                                        NavigationCategories.Type);
                            }
                        }
                    }
                }
            }
        }
    }
}