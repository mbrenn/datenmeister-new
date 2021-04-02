using System;
using System.Collections;
using System.Collections.Generic;
using DatenMeister.Core.Runtime.Workspaces;
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
        private static IEnumerable<ViewExtension> GetForApplicationWindow(
            ViewExtensionInfo viewExtensionInfo)
        {
            var navigationHost = viewExtensionInfo.NavigationHost ??
                                 throw new InvalidOperationException("navigationHost == null");
            
            yield return new ApplicationMenuButtonDefinition(
                "Goto User Forms", async () => await NavigatorForItems.NavigateToItemsInExtent(
                    navigationHost,
                    WorkspaceNames.WorkspaceManagement,
                    WorkspaceNames.UriExtentUserForm),
                string.Empty,
                NavigationCategories.DatenMeisterNavigation);



            yield return new ApplicationMenuButtonDefinition(
                "Goto Management", async () => await NavigatorForExtents.NavigateToExtentList(
                    navigationHost,
                    WorkspaceNames.WorkspaceManagement),
                string.Empty,
                NavigationCategories.DatenMeisterNavigation);

        }
    }
}