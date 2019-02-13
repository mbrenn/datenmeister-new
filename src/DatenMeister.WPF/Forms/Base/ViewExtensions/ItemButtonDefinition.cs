using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Forms.Base.ViewExtensions
{
    public class ItemButtonDefinition : ViewExtension
    {
        public string Name { get; }
        public Action<IObject> OnPressed { get; }

        public override string ToString()
        {
            return $"ItemButtonDefinition: {Name}";
        }
    }
}