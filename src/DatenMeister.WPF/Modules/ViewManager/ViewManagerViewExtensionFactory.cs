using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;

namespace DatenMeister.WPF.Modules.ViewManager
{
    /// <summary>
    /// Contains the factory for the view extensions
    /// </summary>
    public class ViewManagerViewExtensionFactory : IViewExtensionFactory
    {
        /// <summary>
        /// Gets the view extension
        /// </summary>
        /// <param name="viewExtensionTargetInformation"></param>
        /// <returns></returns>
        public IEnumerable<ViewExtension> GetViewExtensions(
            ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            var navigationGuest = viewExtensionTargetInformation.NavigationGuest;
            var itemExplorerControl = navigationGuest as ItemExplorerControl;
            var detailFormControl = viewExtensionTargetInformation.NavigationHost as DetailFormWindow;

            if (navigationGuest is ItemExplorerControl)
            {
                var result = new RibbonButtonDefinition(
                    "Navigate to User Views",
                    () => Navigation.NavigatorForItems.NavigateToItemsInExtent(
                        viewExtensionTargetInformation.NavigationHost,
                        WorkspaceNames.NameManagement,
                        WorkspaceNames.UriUserViewExtent),
                    "",
                    NavigationCategories.Views);

                yield return result;

            }

            if (itemExplorerControl != null|| detailFormControl != null)
            {
                var openView = new RibbonButtonDefinition(
                    "Open View",
                    async () =>
                    {
                        var action = await Navigator.CreateDetailWindow(
                            viewExtensionTargetInformation.NavigationHost,
                            new NavigateToItemConfig
                            {
                                DetailElement = InMemoryObject.CreateEmpty(),
                                FormDefinition = GiveMe.Scope.WorkspaceLogic.GetInternalViewsExtent()
                                    .element("#ViewManagerFindView")
                            });

                        if (action.Result == NavigationResult.Saved && action.DetailElement is IElement asElement)
                        {
                            var formDefinition = asElement.getOrDefault<IElement>("form");

                            itemExplorerControl?.AddTab(
                                itemExplorerControl.Items, new ViewDefinition("Selected Form", formDefinition));

                            detailFormControl?.SetForm(formDefinition);
                        }
                    },
                    "",
                    NavigationCategories.Views);

                yield return openView;
            }
        }
    }
}