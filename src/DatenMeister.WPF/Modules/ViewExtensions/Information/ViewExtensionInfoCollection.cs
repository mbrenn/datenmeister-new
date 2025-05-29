using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information;

/// <summary>
/// Defines the view extension when sent out for a reflective collection
/// </summary>
public class ViewExtensionInfoCollection : ViewExtensionInfo
{
    /// <summary>
    /// Gets or sets the collection being used
    /// </summary>
    public IReflectiveCollection? Collection { get; set; }

    public ViewExtensionInfoCollection(INavigationHost navigationHost, INavigationGuest? navigationGuest) : base(navigationHost, navigationGuest)
    {
    }
}