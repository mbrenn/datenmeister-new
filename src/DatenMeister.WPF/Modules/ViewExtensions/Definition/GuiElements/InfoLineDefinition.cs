#nullable enable

using System;
using System.Windows;

namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.GuiElements
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