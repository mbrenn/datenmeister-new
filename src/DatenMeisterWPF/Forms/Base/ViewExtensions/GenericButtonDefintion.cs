using System;

namespace DatenMeisterWPF.Forms.Base.ViewExtensions
{
    public class GenericButtonDefintion : ViewExtension
    {
        public GenericButtonDefintion(string name, Action onPressed)
        {
            Name = name;
            OnPressed = onPressed;
        }

        public string Name { get; }

        public Action OnPressed { get; }
    }
}