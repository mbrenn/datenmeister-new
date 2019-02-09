using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DatenMeister.Core;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Forms.Base.ViewExtensions;
using DatenMeisterWPF.Forms.Lists;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Modules.TypeManager
{
    public class TypeManagerListView : ItemsInExtentList, INavigationGuest
    {
        public new IEnumerable<ViewExtension> GetViewExtensions()
        {
            var result = base.GetViewExtensions().ToList();

            result.Add( 
                new RibbonButtonDefinition(
                    "Create new Class",
                    () =>
                    {
                        var navigationEvents= NavigatorForItems.NavigateToCreateNewItem(
                            NavigationHost,
                            Extent,
                            Extent.FindInMeta<_UML>(x => x.StructuredClassifiers.__Class));
                        navigationEvents.NewItemCreated += (x, y) => { Extent.elements().add(y.NewItem); };

                        MessageBox.Show("Create new Class");
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