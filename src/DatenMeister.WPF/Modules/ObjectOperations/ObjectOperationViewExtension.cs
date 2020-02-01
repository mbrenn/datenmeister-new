using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.TreeView;
using DatenMeister.WPF.Modules.DefaultTypes;
using DatenMeister.WPF.Navigation;

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
                    "Move object...",
                     async (x) => { await MoveItem(viewExtensionTargetInformation.NavigationHost, x); }) {CategoryName = "Object"};

                yield return new TreeViewItemCommandDefinition(
                    "Copy object...",
                    async (x) => {await CopyItem(viewExtensionTargetInformation.NavigationHost, x); }
                ) {CategoryName = "Object"};
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
                return;
            }

            var options = new CopyOption {CloneAllReferences = false};
            
            var copied = ObjectCopier.Copy(new MofFactory(found), o, options);

            var hints = GiveMe.Scope.Resolve<DefaultClassifierHints>();
            hints.AddToExtentOrElement(found, copied);
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
                return;
            }

            var hints = GiveMe.Scope.Resolve<DefaultClassifierHints>();
            var container = (o as IElement)?.container();
            hints.AddToExtentOrElement(found, o);

            if (container != null)
            {
                hints.RemoveFromExtentOrElement(container, o);
            }

        }
    }
}