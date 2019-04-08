using System.Collections;
using System.Collections.Generic;

namespace DatenMeister.WPF.Forms.Base.ViewExtensions
{
    public interface IViewExtensionFactory
    {
        /// <summary>
        /// Gets the view extensions by the given plugin
        /// </summary>
        /// <param name="viewExtensionTargetInformation">The target to which the view extensions will be applied</param>
        /// <returns>Enumeration of view Extensions</returns>
        IEnumerable<ViewExtension> GetViewExtensions(
            ViewExtensionTargetInformation viewExtensionTargetInformation);
    }
}