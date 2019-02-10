using System;
using System.Windows;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeisterWPF.Forms.Base.ViewExtensions
{
    public class InfoLineDefinition : ViewExtension
    {
        public InfoLineDefinition(Func<UIElement> infolineFactory)
        {
            InfolineFactory = infolineFactory;
        }

        /// <summary>
        /// Gets the factory for the infoline
        /// </summary>
        public Func<UIElement> InfolineFactory { get; }

        public override string ToString()
        {
            return $"InfoLineDefinition";
        }
    }
}