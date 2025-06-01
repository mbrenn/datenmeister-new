using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;

public class RowItemButtonDefinition(
    string name,
    Action<INavigationGuest, IObject> onPressed,
    ItemListViewControl.ButtonPosition position = ItemListViewControl.ButtonPosition.After)
    : ViewExtension
{
    public string Name { get;  } = name;
    public Action<INavigationGuest, IObject> OnPressed { get; } = onPressed;
    public ItemListViewControl.ButtonPosition Position { get; } = position;

    public override string ToString()
    {
        return $"RowItemButtonDefinition: {Name}";
    }
}