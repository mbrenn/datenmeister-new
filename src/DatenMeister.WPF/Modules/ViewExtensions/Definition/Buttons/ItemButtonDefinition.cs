using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;

public class ItemButtonDefinition(string name, Action<IObject> onPressed) : ViewExtension
{
    public string Name { get; } = name;

    public Action<IObject> OnPressed { get; } = onPressed;

    public override string ToString()
    {
        return $"ItemButtonDefinition: {Name}";
    }
}