using System.Collections.Generic;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.TypeManager
{
    public class TypeManagerViewExtension : IViewExtensionFactory
    {
        public IEnumerable<ViewExtension> GetViewExtensions(
            ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            if (viewExtensionTargetInformation.NavigationGuest is ItemExplorerControl itemInExtentList)
            {
                yield return new RibbonButtonDefinition(
                    "View User Types",
                    () => NavigatorForItems.NavigateToItemsInExtent(
                        viewExtensionTargetInformation.NavigationHost,
                        WorkspaceNames.NameTypes,
                        WorkspaceNames.UriUserTypesExtent),
                    string.Empty,
                    "Navigation.User");

                // Inject the buttons to create a new class or a new property (should be done per default, but at the moment per plugin)
                var extent = itemInExtentList.Items.GetAssociatedExtent();
                var extentType = extent?.GetExtentType();
                if (extentType != "Uml.Classes")
                {
                    // The extent type is not of "Uml.Classes", so we don't inject the buttons
                    yield break;
                }

                var selectedPackage = itemInExtentList.SelectedPackage;
                var type = SelectionType.Package;
                if ((selectedPackage as IElement)?.metaclass?.@equals(
                        extent.FindInMeta<_UML>(x => x.StructuredClassifiers.__Class)) == true)
                {
                    type = SelectionType.Class;
                }

                if (type == SelectionType.Package)
                {
                    yield return new RibbonButtonDefinition(
                        "Create new Class",
                        () =>
                        {
                            NavigatorForItems.NavigateToCreateNewItemInExtent(
                                viewExtensionTargetInformation.NavigationHost,
                                extent,
                                extent.FindInMeta<_UML>(x => x.StructuredClassifiers.__Class));
                        },
                        string.Empty,
                        NavigationCategories.Type + "." + "Manager");
                }
                else if (type == SelectionType.Class)
                {
                    yield return
                        new RibbonButtonDefinition(
                            "Create new Property",
                            () =>
                            {
                                NavigatorForItems.NavigateToNewItemForPropertyCollection(
                                    viewExtensionTargetInformation.NavigationHost,
                                    selectedPackage,
                                    _UML._StructuredClassifiers._Class.ownedAttribute,
                                    extent.FindInMeta<_UML>(x => x.Classification.__Property));
                            },
                            string.Empty,
                            NavigationCategories.Type + "." + "Manager");
                }

                yield return new RibbonButtonDefinition(
                    "Create new Package",
                    () =>
                    {
                        if (selectedPackage == null)
                        {
                            NavigatorForItems.NavigateToCreateNewItemInExtent(
                                viewExtensionTargetInformation.NavigationHost,
                                extent,
                                extent.FindInMeta<_UML>(x => x.Packages.__Package));
                        }
                        else
                        {
                            NavigatorForItems.NavigateToNewItemForPropertyCollection(
                                viewExtensionTargetInformation.NavigationHost,
                                itemInExtentList.SelectedPackage,
                                _UML._StructuredClassifiers._Class.ownedAttribute,
                                extent.FindInMeta<_UML>(x => x.Packages.__Package));
                        }

                    },
                    string.Empty,
                    NavigationCategories.Type + "." + "Manager");
            }

        }
    }
}