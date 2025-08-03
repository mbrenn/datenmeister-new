using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;

/// <summary>
/// This menu button is allocated to the shown or selected item and will shown as a menu on this item
/// </summary>
public class ItemMenuButtonDefinition(
    string name,
    Action<IObject> onPressed,
    string? imageName,
    string categoryName,
    int priority = 0)
    : NavigationButtonDefinition(name, NavigationScope.Item, imageName, categoryName, priority)
{
    ///<summary>
    /// Gets the action being executed when the user clicked upon the button
    /// </summary>
    public Action<IObject> OnPressed { get; } = onPressed;
}