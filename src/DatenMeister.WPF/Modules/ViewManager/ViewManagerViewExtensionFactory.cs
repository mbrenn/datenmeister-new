using System.Collections.Generic;
using System.Windows;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Integration;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Navigation;

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
        public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            var result = new RibbonButtonDefinition(
                "Switch to User Views",
                () => Navigation.NavigatorForItems.NavigateToItemsInExtent(
                    viewExtensionTargetInformation.NavigationHost,
                    WorkspaceNames.NameManagement,
                    WorkspaceNames.UriUserViewExtent),
                "",
                "Views");

            yield return result;

            if (viewExtensionTargetInformation.NavigationGuest is ItemExplorerControl)
            {
                var openView = new RibbonButtonDefinition(
                    "Open View",
                    () =>
                    {
                        var action = NavigatorForItems.NavigateToElementDetailView(
                            viewExtensionTargetInformation.NavigationHost,
                            new NavigateToItemConfig
                            {
                                DetailElement = InMemoryObject.CreateEmpty(),
                                FormDefinition = GiveMe.Scope.WorkspaceLogic.GetInternalViewsExtent()
                                    .element("#ViewManagerFindView")
                            });
                        action.Saved += (a, b) =>
                        {
                            var asItemListView =
                                (viewExtensionTargetInformation.NavigationGuest as ItemExplorerControl);
                            asItemListView?.AddTab(
                                asItemListView.Items, new ViewDefinition("Selected Form", b.Item));
                        };
                    },
                    "",
                    "Views");

                yield return openView;
            }
        }
    }
}