using System.Windows;
using Autofac;
using DatenMeister.Integration;
using DatenMeister.Provider.HelpingExtents;
using DatenMeister.Provider.ManagementProviders;
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
        public void SetContent(IDatenMeisterScope scope)
        {
            var workspaceView = scope.Resolve<ViewDefinitions>();

            var workspaceExtent = ManagementProviderHelper.GetExtentsForWorkspaces(scope);
            SetContent(scope, workspaceExtent.elements(), workspaceView.GetWorkspaceListForm());
            SupportNewItems = false;
            AddDefaultButtons();
            AddRowItemButton("Extents", (workspace) =>
            {
                var events = Navigator.TheNavigator.NavigateTo(
                Window.GetWindow(this),
                () =>
                {
                    var dlg = new ExtentList();
                    dlg.SetContent(scope, workspace.get("id").ToString());
                    return dlg;
                });

                events.Closed += (x, y) => UpdateContent();

            });


            AddGenericButton("New Workspace", () =>
            {
                var events = Navigator.TheNavigator.NavigateTo(
                    Window.GetWindow(this),
                    () =>
                    {
                        var dlg = new NewWorkspaceControl();
                        dlg.SetContent(scope);
                        return dlg;
                    });

                events.Closed += (x, y) => UpdateContent();
            });

            AddItemButton("Open Workspace", (x) => { });
        }
    }
}