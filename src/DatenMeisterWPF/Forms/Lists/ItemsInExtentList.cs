using System.Collections.Generic;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ItemsInExtentList : ListViewControl, INavigationGuest
    {
        private string _workspaceId;
        private string _extentUrl;
        private IExtent _extent;

        /// <inheritdoc />
        /// <summary>
        /// Gets the enumeration of all views that may match to the shown items
        /// </summary>
        public override IEnumerable<IElement> GetFormsForView()
        {
            return App.Scope.Resolve<IViewFinder>().FindViews((Items as IHasExtent)?.Extent as IUriExtent, null);
        }

        /// <summary>
        /// Sets the items of the given extent
        /// </summary>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="extentUrl">Url of the extent to be used</param>
        public void SetContent(string workspaceId, string extentUrl)
        {
            _workspaceId = workspaceId;
            _extentUrl = extentUrl;
            var workLogic = App.Scope.Resolve<IWorkspaceLogic>();
            workLogic.FindExtentAndWorkspace(workspaceId, extentUrl, out var workspace, out _extent);
            if (_extent == null)
            {
                MessageBox.Show("The given workspace and extent was not found.");
                return;
            }

            SetContent(_extent.elements(), null);

            AddGenericButton("New Item", () =>
            {
                var elements = Navigator.TheNavigator.NavigateToNewItem(NavigationHost, _extent.elements());
                elements.Closed += (x, y) =>
                {
                    UpdateContent();
                };
            });

            AddDefaultButtons();

            AddRowItemButton(
                "Delete",
                item =>
                {
                    if (MessageBox.Show(
                            "Are you sure to delete the item?", "Confirmation", MessageBoxButton.YesNo) ==
                        MessageBoxResult.Yes)
                    {
                        _extent.elements().remove(item);
                        SetContent(_extent.elements(), null);
                    }
                });
        }

        /// <summary>
        /// Prepares the navigation of the host. The function is called by the navigation 
        /// host. 
        /// </summary>
        public new void PrepareNavigation()
        {
            base.PrepareNavigation();

            NavigationHost.AddNavigationButton(
                "To Extent",
                () => Navigator.TheNavigator.NavigateToExtentList(NavigationHost, _workspaceId),
                Icons.ExtentsShow,
                NavigationCategories.File + ".Workspaces");

            NavigationHost.AddNavigationButton(
                "Extent Info",
                () => Navigator.TheNavigator.OpenExtent(NavigationHost, _workspaceId, _extentUrl),
                null,
                NavigationCategories.File + ".Workspaces");

            NavigationHost.AddNavigationButton(
                "ShowAsTree",
                () =>
                {
                    if (_extent != null)
                    {
                        var window = new TreeViewWindow();
                        window.SetDefaultProperties();
                        window.SetCollection(_extent.elements());
                        window.Show();
                    }
                }, 
                null,
                NavigationCategories.File + ".Views");
        }
    }
}