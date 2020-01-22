using System;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.Buttons;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.FormManager
{
    public partial class FormManagerViewExtension
    {
        /// <summary>
        /// Gets the navigation for the application window
        /// </summary>
        /// <param name="viewExtensionTargetInformation"></param>
        /// <returns></returns>
        private static ViewExtension GetForApplicationWindow(
            ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            var navigationHost = viewExtensionTargetInformation.NavigationHost ??
                                 throw new InvalidOperationException("navigationHost == null");
            
            var result = new ApplicationMenuButtonDefinition(
                "Goto User Forms", async () => await NavigatorForItems.NavigateToItemsInExtent(
                    navigationHost,
                    WorkspaceNames.NameManagement,
                    WorkspaceNames.UriUserFormExtent),
                string.Empty,
                NavigationCategories.DatenMeisterNavigation);

            return result;
        }
    }
}