using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Information;

/// <summary>
/// This class is used when an item list is required to show all properties of the given item.
/// Here, the plugins are asked whether a view extension shall be shown for these items.
/// </summary>
public class ViewExtensionItemPropertiesInformation(
    INavigationHost navigationHost,
    INavigationGuest navigationGuest,
    IObject value,
    string property)
    : ViewExtensionInfo(navigationHost, navigationGuest)
{
    /// <summary>
    /// Gets or sets the value which is intended to be shown
    /// </summary>
    public IObject Value { get; set; } = value;

    /// <summary>
    /// Gets or sets the property which is queried to be shown
    /// </summary>
    public string Property { get; set; } = property;
}

/// <summary>
/// This class is used when an item list is required to show the children of the extents.
/// Here, the plugins are asked whether a view extension shall be shown for these items
/// </summary>
public class ViewExtensionExtentInformation(
    INavigationHost navigationHost,
    INavigationGuest navigationGuest,
    IExtent extent)
    : ViewExtensionInfo(navigationHost, navigationGuest)
{
    /// <summary>
    /// Gets or sets the extent
    /// </summary>
    public IExtent Extent { get; set; } = extent;
}