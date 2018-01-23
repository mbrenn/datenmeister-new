using System;

namespace DatenMeisterWPF.Navigation
{
    public class ControlNavigation : IControlNavigation
    {
        public event EventHandler Closed;

        public virtual void OnClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }

    [Obsolete]
    public class ControlNavigationNewItem : ControlNavigation, IControlNavigationNewItem
    {
        public ControlNavigationNewItem()
        {
            
        }

        public ControlNavigationNewItem(IControlNavigation subItem)
        {
            subItem.Closed += (x, y) => OnClosed();
        }

        public event EventHandler<NewItemEventArgs> NewItemCreated;

        public virtual void OnNewItemCreated(NewItemEventArgs e)
        {
            NewItemCreated?.Invoke(this, e);
        }

        public void Attach(IControlNavigation subItem)
        {
            subItem.Closed += (x, y) => OnClosed();
        }
    }
}