using System.Collections.Generic;
using System.Windows;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.TreeView;

namespace DatenMeister.WPF.Modules.ObjectOperations
{
    public class ObjectOperationViewExtension : IViewExtensionFactory
    {
        public IEnumerable<ViewExtension> GetViewExtensions(ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            if (viewExtensionTargetInformation.IsItemsInExtentExplorerControl())
            {
                yield return new TreeViewItemCommandDefinition(
                    "Move object...",
                    (x) => MessageBox.Show("Move")
                ) {CategoryName = "Object"};
                
                yield return new TreeViewItemCommandDefinition(
                    "Copy object...",
                    (x) => MessageBox.Show("Copy")
                ) {CategoryName = "Object"};
            }
        }
    }
}