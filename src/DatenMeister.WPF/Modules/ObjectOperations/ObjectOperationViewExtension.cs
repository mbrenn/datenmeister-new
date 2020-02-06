using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.TreeView;
using DatenMeister.WPF.Navigation;
using MessageBox = System.Windows.MessageBox;

namespace DatenMeister.WPF.Modules.ObjectOperations
{
    public class ObjectOperationViewExtension : IViewExtensionFactory
    {
        public IEnumerable<ViewExtension> GetViewExtensions(
            ViewExtensionTargetInformation viewExtensionTargetInformation)
        {
            if (viewExtensionTargetInformation.IsItemsInExtentExplorerControl())
            {
                yield return new TreeViewItemCommandDefinition(
                    "Move...",
                     async (x) => { await MoveItem(viewExtensionTargetInformation.NavigationHost, x); }
                    ) {CategoryName = "Item"};

                yield return new TreeViewItemCommandDefinition(
                    "Copy...",
                    async (x) => {await CopyItem(viewExtensionTargetInformation.NavigationHost, x); }
                ) {CategoryName = "Item"};
                
                yield return new TreeViewItemCommandDefinition(
                    "Delete...", (x) => {DeleteItem(viewExtensionTargetInformation.NavigationHost, x); }
                ) {CategoryName = "Item"};
            }
        }

        private async Task CopyItem(INavigationHost navigationHost, IObject? o)
        {
            if (o == null)
            {
                return;
            }

            var extent = o.GetExtentOf();
            var found = await NavigatorForDialogs.Locate(
                navigationHost,
                extent?.GetWorkspace(),
                extent);

            if (found == null)
            {
                // Nothing selected
                return;
            }

            var objectOperation = GiveMe.Scope.Resolve<DatenMeister.Modules.DefaultTypes.ObjectOperations>();
            objectOperation.CopyObject(o, found);
        }

        private async Task MoveItem(INavigationHost navigationHost, IObject? o)
        {
            if (o == null)
            {
                return;
            }

            var extent = o.GetExtentOf();
            var found = await NavigatorForDialogs.Locate(
                navigationHost,
                extent?.GetWorkspace(),
                extent);

            if (found == null)
            {
                // Nothing selected
                return;
            }

            var objectOperation = GiveMe.Scope.Resolve<DatenMeister.Modules.DefaultTypes.ObjectOperations>();
            objectOperation.MoveObject(o, found);

        }

        private void DeleteItem(INavigationHost navigationHost, IObject? o)
        {
            if (o == null)
            {
                return;
            }

            if (
                MessageBox.Show(
                    $"Shall the item '{NamedElementMethods.GetName(o)}' be deleted?",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
            
                var extent = o.GetExtentOf();
                var hints = GiveMe.Scope.Resolve<DefaultClassifierHints>();
                var container = (o as IElement)?.container();

                if (container != null || extent != null)
                {
                    hints.RemoveFromExtentOrElement(container ?? (IObject) extent!, o);
                }

            }
        }
    }
}