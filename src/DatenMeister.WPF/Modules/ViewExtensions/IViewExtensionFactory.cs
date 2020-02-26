using System.Collections.Generic;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Information;

namespace DatenMeister.WPF.Modules.ViewExtensions
{
    public interface IViewExtensionFactory
    {
        /// <summary>
        /// Gets the view extensions by the given plugin
        /// </summary>
        /// <param name="viewExtensionInfo">The target to which the view extensions will be applied</param>
        /// <returns>Enumeration of view Extensions</returns>
        IEnumerable<ViewExtension> GetViewExtensions(
            ViewExtensionInfo viewExtensionInfo);
    }
}