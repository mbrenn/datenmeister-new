using System.Windows;

namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.GuiElements;

public class InfoLineDefinition(Func<UIElement> infolineFactory) : ViewExtension
{
    /// <summary>
    /// Gets the factory for the infoline
    /// </summary>
    public Func<UIElement> InfolineFactory { get; } = infolineFactory;

    public override string ToString()
    {
        return "InfoLineDefinition";
    }
}