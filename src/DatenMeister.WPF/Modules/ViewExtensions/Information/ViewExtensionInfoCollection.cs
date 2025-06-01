using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information;

/// <summary>
/// Defines the view extension when sent out for a reflective collection
/// </summary>
public class ViewExtensionInfoCollection(INavigationHost navigationHost, INavigationGuest? navigationGuest)
    : ViewExtensionInfo(navigationHost, navigationGuest)
{
    /// <summary>
    /// Gets or sets the collection being used
    /// </summary>
    public IReflectiveCollection? Collection { get; set; }
}