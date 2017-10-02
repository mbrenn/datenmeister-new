using System.Windows;
using Autofac;
using DatenMeister.Integration;
using DatenMeister.Provider.HelpingExtents;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Dialogs;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Forms
{
    public class WorkspaceViewControl : ElementListViewControl
    {
        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        /// <param name="scope"></param>
        public void SetContent(IDatenMeisterScope scope)
        {
            var workspaceView = scope.Resolve<WorkspaceViewDefinition>();

            var workspaceExtent = ManagementProviderHelper.GetExtentsForWorkspaces(scope);
            SetContent(scope, workspaceExtent.elements(), workspaceView.CreateForm());
            SupportNewItems = false;
            AddDefaultButtons();


            AddGenericButton("New Workspace", () =>
            {
                var dlg = new NewWorkspaceDialog();
                dlg.SetContent(scope);
                dlg.Owner = Window.GetWindow(this);
                dlg.Show();
                dlg.Closed += (x, y) => UpdateContent();

            });

            AddItemButton("Open Workspace", (x) => { });
        }
    }
}