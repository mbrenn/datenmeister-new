using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Forms.Detail;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class WorkspaceList : ElementListViewControl, INavigationGuest
    {
        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        /// <param name="scope"></param>
        public void SetContent(IElement formElement = null)
        {
            var workspaceExtent = ManagementProviderHelper.GetExtentsForWorkspaces(App.Scope);
            SetContent(workspaceExtent.elements(), formElement);

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
                NavigationCategories.File);

            base.PrepareNavigation();
        }
    }
}