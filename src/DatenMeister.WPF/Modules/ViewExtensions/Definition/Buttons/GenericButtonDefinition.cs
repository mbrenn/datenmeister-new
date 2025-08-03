namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;

public class GenericButtonDefinition(string name, Action onPressed) : ViewExtension
{
    public string Name { get; } = name;

    public Action OnPressed { get; } = onPressed;

    public override string ToString()
    {
        return $"GenericButtonDefinition: {Name}";
    }
}