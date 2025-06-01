namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;

/// <summary>
/// This definition is shown generically to the complete application and does not have a reference to
/// a shown extent or item
/// </summary>
public class ApplicationMenuButtonDefinition(
    string name,
    Action onPressed,
    string? imageName,
    string categoryName,
    int priority = 0)
    : NavigationButtonDefinition(name, NavigationScope.Application, imageName, categoryName, priority)
{
    ///<summary>
    /// Gets the action being executed when the user clicked upon the button
    /// </summary>
    public Action OnPressed { get; } = onPressed;
}