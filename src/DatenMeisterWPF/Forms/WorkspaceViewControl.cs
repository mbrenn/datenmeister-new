using System.Linq;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.HelpingExtents;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Forms.Base;
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
            var workspaceView = scope.Resolve<WorkspaceView>();

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
            });

            AddItemButton("Open Workspace", (x) => { });
        }
    }
}