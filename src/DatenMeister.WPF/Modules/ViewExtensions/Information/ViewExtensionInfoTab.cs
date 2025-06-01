using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information;

/// <summary>
/// This class is instantiated for each tab of the ItemExplorerTab.
/// The included form definition describes the form that will be used to describe form in the tab.
/// It can be a DetailForm or a ListForm.
///
/// The NavigationHost is the host in which the form is shown (most probably the ApplicationWindow)
/// The NavigationGuest is the ItemExplorerInstance
/// </summary>
public class ViewExtensionInfoTab(INavigationHost navigationHost, INavigationGuest navigationGuest)
    : ViewExtensionInfo(navigationHost, navigationGuest)
{
    /// <summary>
    /// Gets or sets the definition for the tab
    /// </summary>
    public IElement? TabFormDefinition { get; set; }
}