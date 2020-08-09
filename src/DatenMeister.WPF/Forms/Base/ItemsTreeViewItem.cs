using System.Windows.Controls;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.TreeView;

namespace DatenMeister.WPF.Forms.Base
{
    public class ItemsTreeViewItem : TreeViewItem
    {
        /// <summary>
        /// Stores the TreeViewItem Parameter of the instance
        /// </summary>
        public TreeViewItemParameter? TreeViewItemParameter { get; set; }
    }
}