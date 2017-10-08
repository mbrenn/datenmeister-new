using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.HelpingExtents;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Forms.Detail;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class WorkspaceList : ElementListViewControl
    {
        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        /// <param name="scope"></param>
        public void SetContent(IDatenMeisterScope scope, IElement formElement = null)
        {
            var workspaceExtent = ManagementProviderHelper.GetExtentsForWorkspaces(scope);
            SetContent(scope, workspaceExtent.elements(), formElement);

            AddDefaultButtons();
            AddRowItemButton("Show Extents", ShowExtents);
            AddRowItemButton("Delete Workspace", DeleteWorkspace);
            AddGenericButton("New Workspace", NewWorkspace);
            
            void ShowExtents(IObject workspace)
            {
                var workspaceId = workspace.get("id").ToString();
                var events = Navigator.TheNavigator.NavigateToExtentList(Window.GetWindow(this), scope, workspaceId);
                
                events.Closed += (x, y) => UpdateContent();
            }

            void NewWorkspace()
            {
                var events = Navigator.TheNavigator.NavigateTo(Window.GetWindow(this), () =>
                {
                    var dlg = new NewWorkspaceControl();
                    dlg.SetContent(scope);
                    return dlg;
                });

                events.Closed += (x, y) => UpdateContent();
            }

            void DeleteWorkspace(IObject workspace)
            {
                if (MessageBox.Show(
                        "Are you sure to delete the workspace? All included extents will also be deleted.", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var workspaceId = workspace.get("id").ToString();

                    var workspaceLogic = scope.Resolve<IWorkspaceLogic>();
                    workspaceLogic.RemoveWorkspace(workspaceId);

                    UpdateContent();
                }
            }
        }
    }
}