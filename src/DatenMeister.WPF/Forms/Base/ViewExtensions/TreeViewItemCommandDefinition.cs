using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Forms.Base.ViewExtensions
{
    public class TreeViewItemCommandDefinition : ViewExtension
    {
        /// <summary>
        /// Gets or sets the text of the tree view item
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the action that shall be executed upon click
        /// </summary>
        public Action<IObject> Action { get; set; }

        public TreeViewItemCommandDefinition()
        {
        }

        public TreeViewItemCommandDefinition(string text, Action<IObject> onClick)
        {
            Text = text;
            Action = onClick;
        }
    }
}