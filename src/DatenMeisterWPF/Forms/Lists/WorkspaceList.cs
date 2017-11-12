using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Detail;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class WorkspaceList : ElementListViewControl, INavigationGuest
    {
        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        public void SetContent()
        {
            // Finds the view
            var viewLogic = App.Scope.Resolve<ViewLogic>();
            var formElement = NamedElementMethods.GetByFullName(
                viewLogic.GetViewExtent(),
                ViewDefinitions.PathWorkspaceListView);

            // Sets the workspaces
            var workspaceExtent = ManagementProviderHelper.GetExtentsForWorkspaces(App.Scope);
            SetContent(workspaceExtent.elements(), formElement);

            // Adds the buttons
            AddDefaultButtons();
            AddRowItemButton("Show Extents", ShowExtents);
            AddRowItemButton("Delete Workspace", DeleteWorkspace);
            
            void ShowExtents(IObject workspace)
            {
                var workspaceId = workspace.get("id").ToString();
                var events = Navigator.TheNavigator.NavigateToExtentList(NavigationHost, workspaceId);
                
                events.Closed += (x, y) => UpdateContent();
            }

            void DeleteWorkspace(IObject workspace)
            {
                if (MessageBox.Show(
                        "Are you sure to delete the workspace? All included extents will also be deleted.", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var workspaceId = workspace.get("id").ToString();

                    var workspaceLogic = App.Scope.Resolve<IWorkspaceLogic>();
                    workspaceLogic.RemoveWorkspace(workspaceId);

                    UpdateContent();
                }
            }
        }

        public new void PrepareNavigation()
        {
            void NewWorkspace()
            {
                var events = Navigator.TheNavigator.NavigateTo(NavigationHost, () =>
                    {
                        var dlg = new NewWorkspaceControl();
                        dlg.SetContent(App.Scope);
                        return dlg;
                    },
                    NavigationMode.Detail);

                events.Closed += (x, y) => UpdateContent();
            }

            NavigationHost.AddNavigationButton(
                "Add Workspace",
                NewWorkspace,
                "workspaces-new",
                NavigationCategories.File + "." + "Workspaces");

            base.PrepareNavigation();
        }
    }
}