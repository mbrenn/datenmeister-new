using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information;

public class ViewExtensionInfoItem(INavigationHost navigationHost, INavigationGuest? navigationGuest)
    : ViewExtensionInfo(navigationHost, navigationGuest)
{
    /// <summary>
    /// Gets or sets the item to be evaluated
    /// </summary>
    public IObject? Item { get; set; }
}