using System.Windows;

namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;

public class FreeWpfElementDefinition : ViewExtension
{
    /// <summary>
    /// Gets or sets the factory element being used to create the WPF element. 
    /// </summary>
    public Func<UIElement>? ElementFactory { get; set; }
}