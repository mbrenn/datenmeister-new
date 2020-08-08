using System;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.FormManager
{
    public partial class FormManagerViewExtension
    {
        /// <summary>
        /// Gets the navigation for the application window
        /// </summary>
        /// <param name="viewExtensionInfo"></param>
        /// <returns></returns>
        private static ViewExtension GetForApplicationWindow(
            ViewExtensionInfo viewExtensionInfo)
        {
            var navigationHost = viewExtensionInfo.NavigationHost ??
                                 throw new InvalidOperationException("navigationHost == null");
            
            var result = new ApplicationMenuButtonDefinition(
                "Goto User Forms", async () => await NavigatorForItems.NavigateToItemsInExtent(
                    navigationHost,
                    WorkspaceNames.WorkspaceManagement,
                    WorkspaceNames.UriExtentUserForm),
                string.Empty,
                NavigationCategories.DatenMeisterNavigation);

            return result;
        }
    }
}