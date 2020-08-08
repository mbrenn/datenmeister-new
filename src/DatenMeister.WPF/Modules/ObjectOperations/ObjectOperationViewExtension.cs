using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Modules.ViewExtensions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.TreeView;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;
using MessageBox = System.Windows.MessageBox;

namespace DatenMeister.WPF.Modules.ObjectOperations
{
    public class ObjectOperationViewExtension : IViewExtensionFactory
    {
        public IEnumerable<ViewExtension> GetViewExtensions(
            ViewExtensionInfo viewExtensionInfo)
        {
            if (viewExtensionInfo.GetItemExplorerControl() != null)
            {
                yield return new TreeViewItemCommandDefinition(
                    "Move...",
                    async (x) => { await MoveItem(viewExtensionInfo.NavigationHost, x); }
                ) {CategoryName = "Item"};

                yield return new TreeViewItemCommandDefinition(
                    "Copy...",
                    async (x) => { await CopyItem(viewExtensionInfo.NavigationHost, x); }
                ) {CategoryName = "Item"};

                yield return new TreeViewItemCommandDefinition(
                        "Edit...", (x) => { EditItem(viewExtensionInfo.NavigationHost, x); }
                    ) {CategoryName = "Item"};

                yield return new TreeViewItemCommandDefinition(
                    "Delete...", (x) => { DeleteItem(x); }
                ) {CategoryName = "Item"};

                yield return new TreeViewItemCommandDefinition(
                    "Copy as Xmi...", (x) => { CopyAsXmi(viewExtensionInfo.NavigationHost, x); }
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
                new NavigatorForDialogs.NavigatorForDialogConfiguration
                {
                    DefaultWorkspace = extent?.GetWorkspace(),
                    DefaultExtent = extent
                });

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
                new NavigatorForDialogs.NavigatorForDialogConfiguration
                {
                    DefaultWorkspace = extent?.GetWorkspace(),
                    DefaultExtent = extent
                });

            if (found == null)
            {
                // Nothing selected
                return;
            }

            var objectOperation = GiveMe.Scope.Resolve<DatenMeister.Modules.DefaultTypes.ObjectOperations>();
            objectOperation.MoveObject(o, found);

        }

        private void DeleteItem(IObject? o)
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
                var container = (o as IElement)?.container();

                if (container != null || extent != null)
                {
                    DefaultClassifierHints.RemoveFromExtentOrElement(container ?? (IObject) extent!, o);
                }

            }
        }

        private async void EditItem(INavigationHost navigationHost, IObject? o)
        {
            if (o == null)
            {
                return;
            }

            try
            {
                await NavigatorForItems.NavigateToElementDetailView(navigationHost, o);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void CopyAsXmi(INavigationHost navigationHost, IObject? o)
        {
            if (o == null)
            {
                return;
            }

            var itemDialog = new ItemXmlViewWindow
            {
                IgnoreIDs = true,
                Owner = navigationHost.GetWindow(),
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            itemDialog.UpdateContent(o);
            itemDialog.Show();
            itemDialog.CopyToClipboard();
            MessageBox.Show(
                itemDialog,
                "Content copied to clipboard",
                "Done",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}