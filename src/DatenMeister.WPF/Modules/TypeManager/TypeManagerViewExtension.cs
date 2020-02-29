using System;
using System.Collections.Generic;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
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
        public IEnumerable<ViewExtension> GetViewExtensions(
            ViewExtensionInfo viewExtensionInfo)
        {
            var navigationHost = viewExtensionInfo.NavigationHost ??
                                 throw new InvalidOperationException("NavigationHost == null");

            if (viewExtensionInfo.NavigationHost is IApplicationWindow)
            {
                yield return new ApplicationMenuButtonDefinition(
                    "Goto User Types", async () => await NavigatorForItems.NavigateToItemsInExtent(
                        navigationHost,
                        WorkspaceNames.NameTypes,
                        WorkspaceNames.UriUserTypesExtent),
                    string.Empty,
                    NavigationCategories.DatenMeisterNavigation);
            }

            if (viewExtensionInfo.NavigationGuest is ItemExplorerControl itemInExtentList)
            {
                // Inject the buttons to create a new class or a new property (should be done per default, but at the moment per plugin)
                var extent = itemInExtentList.RootItem.GetExtentOf();
                if (extent != null)
                {
                    var extentType = extent.GetExtentType();
                    if (extentType == "Uml.Classes")
                    {
                        if (itemInExtentList.IsExtentSelectedInTreeview)
                        {
                            var classMetaClass = extent.FindInMeta<_UML>(x => x.StructuredClassifiers.__Class);
                            if (classMetaClass == null)
                                throw new InvalidOperationException("Class could not be found");

                            yield return new NewInstanceViewDefinition(classMetaClass);

                            yield return new ApplicationMenuButtonDefinition(
                                "Create new Class",
                                () =>
                                    NavigatorForItems.NavigateToCreateNewItemInExtent(
                                        navigationHost,
                                        extent!,
                                        classMetaClass),
                                string.Empty,
                                NavigationCategories.Type + "." + "Manager");
                        }
                    }
                }
            }

            var listControl = viewExtensionInfo.GetListViewForItemsTabForExtentType("Uml.Classes");
            if (listControl != null)
            {
                var extent = listControl.Extent;
                if (extent != null)
                {
                    var classMetaClass = extent.FindInMeta<_UML>(x => x.StructuredClassifiers.__Class);
                    if (classMetaClass == null) throw new InvalidOperationException("Class not found");
                    
                    yield return
                        new CollectionMenuButtonDefinition(
                            "Create new Class",
                            (x) =>
                                NavigatorForItems.NavigateToNewItemForExtent(
                                    navigationHost,
                                    listControl.Extent,
                                    classMetaClass),
                            string.Empty,
                            NavigationCategories.Type);
                }
            }

            if (viewExtensionInfo.NavigationGuest is ItemListViewControl extentList
                && viewExtensionInfo is ViewExtensionItemPropertiesInformation propertiesInformation)
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
                                        (x) =>
                                            NavigatorForItems.NavigateToNewItemForPropertyCollection(
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