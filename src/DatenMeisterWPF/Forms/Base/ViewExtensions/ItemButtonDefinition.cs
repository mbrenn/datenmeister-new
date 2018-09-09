using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeisterWPF.Forms.Base.ViewExtensions
{
    public class ItemButtonDefinition : ViewExtension
    {
        public string Name { get; }
        public Action<IElement> OnPressed { get; }
    }
}