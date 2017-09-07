using System.Linq;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Forms
{
    public class WorkspaceViewControl : ListViewControl
    {
        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        /// <param name="scope"></param>
        public void Show(IDatenMeisterScope scope)
        {
            var workspaceLogic = scope.Resolve<IWorkspaceLogic>();
            var workspaces = workspaceLogic.Workspaces.Cast<IObject>().ToList();
            var workspaceView = scope.Resolve<WorkspaceView>();

            AddItemButton("Open Workspace", (x) => MessageBox.Show("TEST" + ((Workspace) x).id));

            var formDefinition = workspaceView.CreateForm();

            UpdateContent(scope, workspaces, formDefinition);
        }
    }
}