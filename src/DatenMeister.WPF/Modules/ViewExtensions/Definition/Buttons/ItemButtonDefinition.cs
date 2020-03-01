#nullable enable

using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons
{
    public class ItemButtonDefinition : ViewExtension
    {
        public ItemButtonDefinition(string name, Action<IObject> onPressed)
        {
            Name = name;
            OnPressed = onPressed;
        }

        public string Name { get; }

        public Action<IObject> OnPressed { get; }

        public override string ToString()
        {
            return $"ItemButtonDefinition: {Name}";
        }
    }
}