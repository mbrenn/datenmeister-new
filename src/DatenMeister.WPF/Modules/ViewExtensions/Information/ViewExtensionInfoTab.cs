using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information
{
    /// <summary>
    /// This class is instantiated for each tab of the ItemExplorerTab
    /// </summary>
    public class ViewExtensionInfoTab : ViewExtensionInfo
    {
        /// <summary>
        /// Gets or sets the definition for the tab
        /// </summary>
        public IElement? TabFormDefinition { get; set; }

        public ViewExtensionInfoTab(INavigationHost navigationHost, INavigationGuest navigationGuest) 
            : base(navigationHost, navigationGuest)
        {
        }
    }
}