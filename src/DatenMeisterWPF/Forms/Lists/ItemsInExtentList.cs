using System.Collections.Generic;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Modules;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ItemsInExtentList : ElementListViewControl, INavigationGuest
    {
        private string _workspaceId;
        private IDatenMeisterScope _scope;

        /// <inheritdoc />
        /// <summary>
        /// Gets the enumeration of all views that may match to the shown items
        /// </summary>
        public override IEnumerable<IElement> GetFormsForView()
        {
            return _scope.Resolve<IViewFinder>().FindViews(null, null);
        }

        public void SetContent(IDatenMeisterScope scope, string workspaceId, string extentUrl)
        {
            _scope = scope;
            _workspaceId = workspaceId;
            var workLogic = scope.Resolve<IWorkspaceLogic>();
            var extent = workLogic.FindExtent(workspaceId, extentUrl);
            if (extent == null)
            {
                MessageBox.Show("The given workspace and extent was not found.");
                return;
            }

            SetContent(extent.elements(), null);

            AddGenericButton("New Item", () =>
            {
                var mofFactory = new MofFactory(extent);
                var newElement = mofFactory.create(null);
                var elements = Navigator.TheNavigator.NavigateToElementDetailView(
                    NavigationHost, newElement);
                elements.Closed += (x, y) =>
                {
                    extent.elements().add(newElement);
                    RefreshViewDefinition();
                    UpdateContent();
                };
            });

            AddDefaultButtons();
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
        }
    }
}