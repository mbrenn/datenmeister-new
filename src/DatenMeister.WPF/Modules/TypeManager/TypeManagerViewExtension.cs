using System.Collections.Generic;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.Buttons;
using DatenMeister.WPF.Forms.Base.ViewExtensions.ListViews;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.TypeManager
{
    public class TypeManagerViewExtension : IViewExtensionFactory
    {
        public IEnumerable<ViewExtension> GetViewExtensions(
            ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            if (viewExtensionTargetInformation.NavigationHost is IApplicationWindow)
            {
                yield return new ApplicationMenuButtonDefinition(
                    "Goto User Types",
                    () => NavigatorForItems.NavigateToItemsInExtent(
                        viewExtensionTargetInformation.NavigationHost,
                        WorkspaceNames.NameTypes,
                        WorkspaceNames.UriUserTypesExtent),
                    string.Empty,
                    NavigationCategories.DatenMeisterNavigation);
            }

            if (viewExtensionTargetInformation.NavigationGuest is ItemExplorerControl itemInExtentList)
            {
                // Inject the buttons to create a new class or a new property (should be done per default, but at the moment per plugin)
                var extent = itemInExtentList.RootItem.GetExtentOf();
                var extentType = extent?.GetExtentType();
                if (extentType == "Uml.Classes")
                {
                    if (itemInExtentList.IsExtentSelectedInTreeview)
                    {
                        var classMetaClass = extent.FindInMeta<_UML>(x => x.StructuredClassifiers.__Class);

                        yield return new NewInstanceViewDefinition(classMetaClass);

                        yield return new ApplicationMenuButtonDefinition(
                            "Create new Class",
                            () =>
                                NavigatorForItems.NavigateToCreateNewItemInExtent(
                                    viewExtensionTargetInformation.NavigationHost,
                                    extent,
                                    classMetaClass),
                            string.Empty,
                            NavigationCategories.Type + "." + "Manager");
                        
                        yield return
                            new CollectionMenuButtonDefinition(
                                "Create new Class",
                                (x) =>
                                    NavigatorForItems.NavigateToNewItemForExtent(
                                        viewExtensionTargetInformation.NavigationHost,
                                        extent,
                                        classMetaClass),
                                string.Empty,
                                NavigationCategories.Type);
                    }
                }
            }

            
            if (viewExtensionTargetInformation.NavigationGuest is ItemListViewControl extentList
                && viewExtensionTargetInformation is ViewExtensionForItemPropertiesInformation propertiesInformation)
            {
                var selectedPackage = propertiesInformation.Value as IElement;
                var extent = selectedPackage.GetExtentOf();

                var classMetaClass = extent.FindInMeta<_UML>(x => x.StructuredClassifiers.__Class);
                var propertyName = propertiesInformation.Property;
                if (selectedPackage?.metaclass?.@equals(classMetaClass) == true
                    && propertyName == _UML._StructuredClassifiers._Class.ownedAttribute)
                {
                    var propertyMetaClass = extent.FindInMeta<_UML>(x => x.Classification.__Property);

                    yield return new NewInstanceViewDefinition(propertyMetaClass);

                    yield return
                        new CollectionMenuButtonDefinition(
                            "Create new Property",
                            (x) =>
                                NavigatorForItems.NavigateToNewItemForPropertyCollection(
                                    viewExtensionTargetInformation.NavigationHost,
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