using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeisterWPF.Forms.Base.ViewExtensions
{
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
        public ItemListViewControl.ButtonPosition Position { get;  }
    }
}