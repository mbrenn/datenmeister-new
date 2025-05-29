using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Navigation;

public class ControlNavigation : IControlNavigation, IControlNavigationSaveItem, IControlNavigationNewObject
{
    public INavigationGuest? NavigationGuest { get; set; }

    public INavigationHost? NavigationHost { get; set; }

    public event EventHandler? Closed;
    public event EventHandler<ItemEventArgs>? Saved;
    public event EventHandler<NewItemEventArgs>? NewObjectCreated;
        
    /// <inheritdoc />
    public bool IsNewObjectCreated { get; set; }

    /// <inheritdoc />
    public IObject? NewObject { get; set; }

    public virtual void OnSaved(ItemEventArgs e)
    {
        Saved?.Invoke(this, e);
    }

    public virtual void OnNewItemCreated(NewItemEventArgs e)
    {
        NewObjectCreated?.Invoke(this, e);
    }

    public virtual void OnClosed()
    {
        Closed?.Invoke(this, EventArgs.Empty);
    }
}