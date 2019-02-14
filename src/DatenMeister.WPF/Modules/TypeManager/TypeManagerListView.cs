using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Lists;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.TypeManager
{
    enum SelectionType
    {
        Package,
        Class
    }
    public class TypeManagerListView : ItemsInExtentList, INavigationGuest
    {
        public new IEnumerable<ViewExtension> GetViewExtensions()
        {
            var result = base.GetViewExtensions().ToList();

            var type = SelectionType.Package;
            if ((SelectedPackage as IElement)?.metaclass?.@equals(
                    Extent.FindInMeta<_UML>(x => x.StructuredClassifiers.__Class)) == true)
            {
                type = SelectionType.Class;
            }

            if (type == SelectionType.Package)
            {
                result.Add(
                    new RibbonButtonDefinition(
                        "Create new Class",
                        () =>
                        {
                            NavigatorForItems.NavigateToCreateNewItemInExtent(
                                NavigationHost,
                                Extent,
                                Extent.FindInMeta<_UML>(x => x.StructuredClassifiers.__Class));
                        },
                        Name = string.Empty,
                        NavigationCategories.Type + "." + "Manager"));
            }
            else
            {
                result.Add(
                    new RibbonButtonDefinition(
                        "Create new Property",
                        () =>
                        {
                            NavigatorForItems.NavigateToNewItemForPropertyCollection(
                                NavigationHost,
                                SelectedPackage,
                                _UML._StructuredClassifiers._Class.ownedAttribute,
                                Extent.FindInMeta<_UML>(x => x.Classification.__Property));
                        },
                        Name = string.Empty,
                        NavigationCategories.Type + "." + "Manager"));

            }

            result.Add(
                new RibbonButtonDefinition(
                    "Create new Package",
                () =>
                {
                    if (SelectedPackage == null)
                    {
                        NavigatorForItems.NavigateToCreateNewItemInExtent(
                            NavigationHost,
                            Extent,
                            Extent.FindInMeta<_UML>(x => x.Packages.__Package));
                    }
                    else
                    {
                        NavigatorForItems.NavigateToNewItemForPropertyCollection(
                            NavigationHost,
                            SelectedPackage,
                            _UML._StructuredClassifiers._Class.ownedAttribute,
                            Extent.FindInMeta<_UML>(x => x.Packages.__Package));
                    }

                },
                    Name = string.Empty,
                    NavigationCategories.Type + "." + "Manager"));

            return result;
        }

        /// <summary>
        /// Opens the Type Manager
        /// </summary>
        /// <param name="window">Window to be used</param>
        /// <returns>The navigation support</returns>
        public static IControlNavigation NavigateToTypeManager(
            INavigationHost window)
        {
            return window.NavigateTo(() =>
                    new TypeManagerListView { WorkspaceId = WorkspaceNames.NameTypes, ExtentUrl = WorkspaceNames.UriUserTypesExtent },
                NavigationMode.List);
        }


    }
}