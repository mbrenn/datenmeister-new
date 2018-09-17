using System.Runtime.Remoting.Messaging;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Forms.Base.ViewExtensions;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ListRequests
    {
        /// <summary>
        /// Requests the form for the workspace
        /// </summary>
        /// <returns>Requested form</returns>
        internal static ViewDefinition RequestFormForWorkspaces()
        {
            // Finds the view
            var viewLogic = App.Scope.Resolve<ViewLogic>();
            var formElement = NamedElementMethods.GetByFullName(
                viewLogic.GetInternalViewExtent(),
                ManagementViewDefinitions.PathWorkspaceListView);
            var viewDefinition = new ViewDefinition("Workspaces", formElement);

            viewDefinition.ViewExtensions.Add(
                new RowItemButtonDefinition("Show Extents", ShowExtents));
            viewDefinition.ViewExtensions.Add(
                new RowItemButtonDefinition("Delete Workspace", DeleteWorkspace));

            return viewDefinition;

            void ShowExtents(INavigationGuest navigationHost, IObject workspace)
            {
                var workspaceId = workspace.get("id")?.ToString();
                if (workspaceId == null)
                {
                    return;
                }

                var events = NavigatorForExtents.NavigateToExtentList(navigationHost.NavigationHost, workspaceId);

                events.Closed += (x, y) => (navigationHost as ItemListViewControl)?.UpdateContent();
            }

            void DeleteWorkspace(INavigationGuest navigationHost, IObject workspace)
            {
                if (MessageBox.Show(
                        "Are you sure to delete the workspace? All included extents will also be deleted.", "Confirmation",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var workspaceId = workspace.get("id").ToString();

                    var workspaceLogic = App.Scope.Resolve<IWorkspaceLogic>();
                    workspaceLogic.RemoveWorkspace(workspaceId);

                    (navigationHost as ItemListViewControl)?.UpdateContent();
                }
            }
        }

        /// <summary>
        /// Requests the form for extent elements
        /// </summary>
        /// <param name="workspaceId">The Id of the workspace</param>
        /// <returns>The created form</returns>
        internal static ViewDefinition RequestFormForExtents(string workspaceId)
        {
            var viewExtent = App.Scope.Resolve<ViewLogic>().GetInternalViewExtent();
            var result =
                NamedElementMethods.GetByFullName(
                    viewExtent,
                    ManagementViewDefinitions.PathExtentListView);

            var viewDefinition = new ViewDefinition("Extents", result);
            viewDefinition.ViewExtensions.Add(new RowItemButtonDefinition("Show Items", ShowItems));

            return viewDefinition;
            
            void ShowItems(INavigationGuest navigationGuest, IObject extentElement)
            {
                var listViewControl = navigationGuest as ItemListViewControl;

                var uri = extentElement.get("uri").ToString();

                var events = NavigatorForItems.NavigateToItemsInExtent(
                    listViewControl.NavigationHost,
                    workspaceId,
                    uri);
                events.Closed += (x, y) => listViewControl.UpdateContent();
            }
        }
    }
}