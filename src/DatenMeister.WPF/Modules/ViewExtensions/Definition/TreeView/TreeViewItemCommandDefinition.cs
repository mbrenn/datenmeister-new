#nullable enable

using System;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.TreeView
{
    public class TreeViewItemCommandDefinition : ViewExtension
    {
        /// <summary>
        /// Gets or sets the text of the tree view item
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the name of the category. This leads to the creation of submenus
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the action that shall be executed upon click
        /// </summary>
        public Action<TreeViewItemParameter> Action { get; set; }

        public TreeViewItemCommandDefinition(string text, Action<TreeViewItemParameter> onClick)
        {
            Text = text;
            Action = onClick;
        }


        public TreeViewItemCommandDefinition(string text, Action<TreeViewItemParameter> onClick, string categoryName)
            : this(text, onClick)
        {
            CategoryName = categoryName;
        }


        /// <summary>
        /// Defines whether the Tree View item is valid for the selected item
        /// </summary>
        /// <returns>true, if the item shall be shown</returns>
        public Func<IObject?, bool>? FilterFunction { get; set; }

        public override string ToString()
        {
            return Text ?? "<no text>";
        }
    }
}