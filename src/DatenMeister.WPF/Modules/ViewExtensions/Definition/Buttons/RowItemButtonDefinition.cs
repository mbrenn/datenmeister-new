using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;

public class RowItemButtonDefinition : ViewExtension
{
    public RowItemButtonDefinition(
        string name,
        Action<INavigationGuest, IObject> onPressed,
        ItemListViewControl.ButtonPosition position = ItemListViewControl.ButtonPosition.After)
    {
        Name = name;
        OnPressed = onPressed;
        Position = position;
    }

    public string Name { get;  }
    public Action<INavigationGuest, IObject> OnPressed { get; }
    public ItemListViewControl.ButtonPosition Position { get; }

    public override string ToString()
    {
        return $"RowItemButtonDefinition: {Name}";
    }
}