using System;

namespace DatenMeister.WPF.Navigation
{
    public class ControlNavigation : IControlNavigation, IControlNavigationSaveItem, IControlNavigationNewItem
    {
        public event EventHandler Closed;
        public event EventHandler<ItemEventArgs> Saved;
        public event EventHandler<NewItemEventArgs> NewItemCreated;

        public virtual void OnSaved(ItemEventArgs e)
        {
            Saved?.Invoke(this, e);
        }

        public virtual void OnNewItemCreated(NewItemEventArgs e)
        {
            NewItemCreated?.Invoke(this, e);
        }

        public virtual void OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}