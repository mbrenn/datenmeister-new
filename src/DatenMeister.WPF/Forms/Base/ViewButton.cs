#nullable enable

using System;
using System.Windows.Controls;

namespace DatenMeister.WPF.Forms.Base
{
    public class ViewButton : Button
    {
        public ViewButton()
        {
            Click += (x, y) => { OnPressed(); };
        }

        public event EventHandler? Pressed;

        protected virtual void OnPressed()
        {
            Pressed?.Invoke(this, EventArgs.Empty);
        }
    }
}