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
}