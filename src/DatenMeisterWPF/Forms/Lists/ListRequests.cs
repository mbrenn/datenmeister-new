using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ListRequests
    {
        /// <summary>
        /// Requests the form for the workspace
        /// </summary>
        /// <param name="listViewControl">List view to which the item shall be added</param>
        /// <returns>Requested form</returns>
        internal static IElement RequestFormForWorkspaces(ListViewControl listViewControl)
        {
            // Finds the view
            var viewLogic = App.Scope.Resolve<ViewLogic>();
            var formElement = NamedElementMethods.GetByFullName(
                viewLogic.GetInternalViewExtent(),
                ManagementViewDefinitions.PathWorkspaceListView);

            // Adds the buttons
            listViewControl.AddDefaultButtons();
            listViewControl.AddRowItemButton("Show Extents", ShowExtents);
            listViewControl.AddRowItemButton("Delete Workspace", DeleteWorkspace);

            void ShowExtents(IObject workspace)
            {
                var workspaceId = workspace.get("id")?.ToString();
                if (workspaceId == null)
                {
                    return;
                }

                var events = NavigatorForExtents.NavigateToExtentList(listViewControl.NavigationHost, workspaceId);

                events.Closed += (x, y) => listViewControl.UpdateContent();
            }

            void DeleteWorkspace(IObject workspace)
            {
                if (MessageBox.Show(
                        "Are you sure to delete the workspace? All included extents will also be deleted.", "Confirmation",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var workspaceId = workspace.get("id").ToString();

                    var workspaceLogic = App.Scope.Resolve<IWorkspaceLogic>();
                    workspaceLogic.RemoveWorkspace(workspaceId);

                    listViewControl.UpdateContent();
                }
            }

            return formElement;
        }

        /// <summary>
        /// Requests the form for extent elements
        /// </summary>
        /// <param name="listViewControl">The list view being used as host for the form</param>
        /// <param name="workspaceId">The Id of the workspace</param>
        /// <returns>The created form</returns>
        internal static IElement RequestFormForExtents(ListViewControl listViewControl, string workspaceId)
        {
            var viewExtent = App.Scope.Resolve<ViewLogic>().GetInternalViewExtent();
            var result =
                NamedElementMethods.GetByFullName(
                    viewExtent,
                    ManagementViewDefinitions.PathExtentListView);
            listViewControl.AddDefaultButtons();
            listViewControl.AddRowItemButton("Show Items", ShowItems);

            void ShowItems(IObject extentElement)
            {
                var uri = extentElement.get("uri").ToString();

                var events = NavigatorForItems.NavigateToItemsInExtent(
                    listViewControl.NavigationHost,
                    workspaceId,
                    uri);
                events.Closed += (x, y) => listViewControl.UpdateContent();
            }

            return result;
        }
    }
}