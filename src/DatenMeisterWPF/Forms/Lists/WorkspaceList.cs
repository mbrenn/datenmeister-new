using System.Windows;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.HelpingExtents;
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
            AddRowItemButton("Show Extents", (workspace) =>
            {
                var events = Navigator.TheNavigator.NavigateToExtentList(
                    Window.GetWindow(this),
                    scope,
                    workspace.get("id").ToString());
               

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