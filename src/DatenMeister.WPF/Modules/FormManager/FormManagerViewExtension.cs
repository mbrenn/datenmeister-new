#nullable enable

using System.Collections.Generic;
using DatenMeister.WPF.Forms;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Windows;

namespace DatenMeister.WPF.Modules.FormManager
{
    /// <summary>
    /// Contains the factory for the view extensions
    /// </summary>
    public partial class FormManagerViewExtension : IViewExtensionFactory
    {
        /// <summary>
        /// Gets the view extension
        /// </summary>
        /// <param name="viewExtensionInfo"></param>
        /// <returns></returns>
        public IEnumerable<ViewExtension> GetViewExtensions(
            ViewExtensionInfo viewExtensionInfo)
        {
            var navigationGuest = viewExtensionInfo.NavigationGuest;
            var navigationHost = viewExtensionInfo.NavigationHost;
            
            var itemExplorerControl = navigationGuest as ItemExplorerControl;
            var detailFormWindow = navigationHost as DetailFormWindow;

            if (navigationHost is IApplicationWindow)
            {
                yield return GetForApplicationWindow(viewExtensionInfo);
            }

            if (detailFormWindow != null)
            {
                foreach (var viewExtension in GetForDetailWindow(
                    viewExtensionInfo,
                    detailFormWindow))
                {
                    yield return viewExtension;
                }
            }

            if (itemExplorerControl != null)
            {
                foreach (var viewExtension in GetForItemExplorerControl(itemExplorerControl))
                {
                    yield return viewExtension;
                }
            }
        }
    }
}